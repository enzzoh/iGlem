using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using test_v01.Models;
using test_v01.Repository;
using test_v01.Repository.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using PdfSharpCore.Pdf.IO;
using A = DocumentFormat.OpenXml.Drawing;



namespace test_v01.Controllers
{
    public class ListaDocumentosController : Controller
    {
        private readonly SITEtccDbContext _context = new SITEtccDbContext();



        public IActionResult ToggleFavorite(int id)
        {
            var documento = _context.Documentos.FirstOrDefault(d => d.Documentoid == id);
            if (documento != null)
            {
                documento.IsFavorite = !documento.IsFavorite; // Alterna o estado de favorito
                _context.SaveChanges();
            }
            return RedirectToAction("HomeList"); // Redireciona para a tela principal
        }

        public IActionResult Favorites()
        {
            var favoritos = _context.Documentos.Where(d => d.IsFavorite).ToList();
            return View(favoritos); // Passa a lista de documentos favoritos para a view
        }


        public IActionResult MarcarFavorito(int id)
        {
            var documento = _context.Documentos.Find(id);
            if (documento != null)
            {
                documento.IsFavorite = true; // Supondo que você tenha uma propriedade 'Favorito' no modelo
                _context.SaveChanges();
            }
            return RedirectToAction("HomeList");
        }


        // Outra ação com parâmetros diferentes, caso necessário
        [HttpGet("home-list/{id}")]
        public IActionResult HomeList(int id)
        {
            var documento = _context.Documentos.FirstOrDefault(d => d.Documentoid == id);
            return View(documento);
        }
        public async Task<IActionResult> HomeList(string searchTerm, string searchType)
        {
            var userIdClaim = User.FindFirst("Idusuario");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            var alteradoTerm = Request.Query["alterTerm"].ToString(); // Captura um parâmetro extra da query string
            if (!string.IsNullOrWhiteSpace(alteradoTerm))
            {
                searchTerm = alteradoTerm; // Modifica o valor de searchTerm
            }

            var documentosQuery = _context.Documentos
                .Where(d => d.Idusuario == userId); // Filtra pelos documentos do usuário logado

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                if (searchType == "name")
                {
                    documentosQuery = documentosQuery.Where(d => d.Documentonome.StartsWith(searchTerm)); // Pesquisa pelo nome
                }
                else if (searchType == "content")
                {
                    documentosQuery = documentosQuery.Where(d => EF.Functions.Like(d.conteudodocumento, $"%{searchTerm}%")); // Pesquisa pelo conteúdo
                }
            }

            // Ordena para que os favoritos apareçam primeiro
            var documentos = await documentosQuery
                .OrderByDescending(d => d.IsFavorite)  // Coloca os favoritos no topo
                .ToListAsync();

            ViewData["SearchTerm"] = searchTerm;
            ViewData["SearchType"] = searchType;

            return documentos != null ? View(documentos) : Problem("Nenhum documento encontrado para este usuário.");
        }



        public async Task<IActionResult> Create()
        {
            return View();
      
        }



        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Valida se o usuário está logado
            var userIdClaim = User.FindFirst("Idusuario");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Verifica se o arquivo foi enviado
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            string fileContent = string.Empty; // Variável para armazenar o conteúdo extraído

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileData = memoryStream.ToArray();
                string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();

                // Processa diferentes tipos de arquivo e extrai o conteúdo
                if (fileExtension == ".xlsx" || fileExtension == ".xls")
                {
                    fileContent = ReadExcelFile(fileData);
                }
                else if (fileExtension == ".docx")
                {
                    fileContent = ReadWordFile(fileData);
                }
                else if (fileExtension == ".txt")
                {
                    fileContent = ReadTextFile(fileData);
                }
                else if (fileExtension == ".pdf")
                {
                    fileContent = ReadPdfFile(fileData);
                }
                else if (fileExtension == ".pptx")
                {
                    fileContent = ReadPptxFile(fileData);
                }

                // Cria e salva o documento no banco de dados
                var documento = new Documento
                {
                    Caminhodocumento = file.FileName,
                    Documentonome = file.FileName,
                    Idusuario = userId,
                    FileData = fileData,
                    conteudodocumento = fileContent, // Atribui o conteúdo extraído à propriedade
                    datacriacao = DateTime.Now
                };

                _context.Documentos.Add(documento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("HomeList");
        }

        public string ReadWordFile(byte[] fileData)
        {
            using (var stream = new MemoryStream(fileData))
            using (var wordDocument = WordprocessingDocument.Open(stream, false))
            {
                var body = wordDocument.MainDocumentPart.Document.Body;
                return body.InnerText;
            }
        }

        public string ReadExcelFile(byte[] fileData)
        {
            // Configura a licença do EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream(fileData))
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                var content = new StringBuilder();

                for (int row = 1; row <= rowCount; row++)
                {
                    var cellValue = worksheet.Cells[row, 1].Text; // Lê o valor da primeira célula
                    content.AppendLine(cellValue); // Adiciona o valor ao conteúdo
                }

                return content.ToString();
            }
        }


        public string ReadTextFile(byte[] fileData)
        {
            return Encoding.UTF8.GetString(fileData);
        }

        public string ReadPdfFile(byte[] fileData)
        {
            var content = new StringBuilder();
            using (var stream = new MemoryStream(fileData))
            using (var pdfDocument = UglyToad.PdfPig.PdfDocument.Open(stream))
            {
                foreach (var page in pdfDocument.GetPages())
                {
                    foreach (var word in page.GetWords())
                    {
                        content.Append(word.Text + " "); // Adiciona um espaço após cada palavra
                    }
                    content.AppendLine(); // Nova linha ao final de cada página
                }
            }
            return content.ToString().Trim();
        }


        public string ReadPptxFile(byte[] fileData)
        {
            var content = new StringBuilder();
            using (var stream = new MemoryStream(fileData))
            using (var pptDocument = PresentationDocument.Open(stream, false))
            {
                var slides = pptDocument.PresentationPart.SlideParts;

                foreach (var slide in slides)
                {
                    var texts = slide.Slide.Descendants<A.Text>().Select(t => t.Text);
                    foreach (var text in texts)
                    {
                        content.AppendLine(text);
                    }
                }
            }
            return content.ToString();
        }


        public async Task<IActionResult> Download(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null || documento.FileData == null)
            {
                return NotFound();
            }

            return File(documento.FileData, "application/octet-stream", documento.Caminhodocumento);
        }

        private bool DocumentoExists(int id)
        {
            return (_context.Documentos?.Any(e => e.Documentoid == id)).GetValueOrDefault();
        }




        public async Task<IActionResult> Edit(int? id)
        {
            var userIdClaim = User.FindFirst("Idusuario");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            if (id == null || _context.Documentos == null)
            {
                return NotFound();
            }

            var documento = await _context.Documentos
                .FirstOrDefaultAsync(d => d.Documentoid == id && d.Idusuario == userId);

            if (documento == null)
            {
                return NotFound();
            }

            // Remove a extensão antes de enviar para a view
            documento.Documentonome = Path.GetFileNameWithoutExtension(documento.Documentonome);

            return View(documento);
        }

        public async Task<IActionResult> Delete(int id)
        {
            // Verifica se o usuário está autenticado e obtém o ID do usuário
            var userIdClaim = User.FindFirst("Idusuario");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Encontra o documento pelo ID e verifica se o documento pertence ao usuário
            var documento = await _context.Documentos
                .FirstOrDefaultAsync(d => d.Documentoid == id && d.Idusuario == userId);

            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Documentoid, Documentonome")] Documento documento)
        {
            if (id != documento.Documentoid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var documentoExistente = await _context.Documentos.FindAsync(id);
                    if (documentoExistente == null)
                    {
                        return NotFound();
                    }

                    // Adiciona a extensão de volta ao nome do documento
                    var extensao = Path.GetExtension(documentoExistente.Caminhodocumento);
                    documentoExistente.Documentonome = documento.Documentonome + extensao;
                    documentoExistente.Caminhodocumento = documentoExistente.Documentonome;

                    _context.Update(documentoExistente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentoExists(documento.Documentoid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(HomeList));
            }
            return View(documento);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HomeList));
        }




    }
}

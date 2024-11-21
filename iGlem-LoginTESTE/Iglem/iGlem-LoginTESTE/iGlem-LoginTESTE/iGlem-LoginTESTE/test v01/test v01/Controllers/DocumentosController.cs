using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test_v01.Repository;
using test_v01.Repository.Models;
using DocumentFormat.OpenXml.Packaging;
using System.Text;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net.Http;

namespace test_v01.Controllers
{
    [Authorize]
    public class DocumentosController : Controller
    {
        private readonly SITEtccDbContext _context = new SITEtccDbContext();


        // GET: Documentos



        public async Task<IActionResult> Index()
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            var sITEtccDbContext = _context.Documentos.Include(d => d.IdusuarioNavigation);
            return View(await sITEtccDbContext.ToListAsync());

        }

        // GET: Documentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (id == null || _context.Documentos == null)
            {
                return NotFound();
            }

            var documento = await _context.Documentos
                .Include(d => d.IdusuarioNavigation)
                .FirstOrDefaultAsync(m => m.Documentoid == id);
            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        // GET: Documentos/Create
        public IActionResult Create()
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            ViewData["Idusuario"] = new SelectList(_context.Usuarios, "Idusuario", "Idusuario");
            return View();
        }

        // POST: Documentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Documentoid,Caminhodocumento,Documentonome,Idusuario,FileData")] Documento documento)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (ModelState.IsValid)
            {
                _context.Add(documento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idusuario"] = new SelectList(_context.Usuarios, "Idusuario", "Idusuario", documento.Idusuario);
            return View(documento);
        }

        // GET: Documentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (id == null || _context.Documentos == null)
            {
                return NotFound();
            }

            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }
            ViewData["Idusuario"] = new SelectList(_context.Usuarios, "Idusuario", "Idusuario", documento.Idusuario);
            return View(documento);
        }

        // POST: Documentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Documentoid,Caminhodocumento,Documentonome,Idusuario,FileData")] Documento documento)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (id != documento.Documentoid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documento);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idusuario"] = new SelectList(_context.Usuarios, "Idusuario", "Idusuario", documento.Idusuario);
            return View(documento);
        }

        // GET: Documentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (id == null || _context.Documentos == null)
            {
                return NotFound();
            }

            var documento = await _context.Documentos
                .Include(d => d.IdusuarioNavigation)
                .FirstOrDefaultAsync(m => m.Documentoid == id);
            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        // POST: Documentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isAdminClaim = User.FindFirst("IsAdmin");
            if (isAdminClaim == null || !bool.Parse(isAdminClaim.Value))
            {
                return Forbid(); // Retorna um erro 403 - Acesso proibido
            }
            if (_context.Documentos == null)
            {
                return Problem("Entity set 'SITEtccDbContext.Documentos'  is null.");
            }
            var documento = await _context.Documentos.FindAsync(id);
            if (documento != null)
            {
                _context.Documentos.Remove(documento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentoExists(int id)
        {
            return (_context.Documentos?.Any(e => e.Documentoid == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int? idusuario)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileData = memoryStream.ToArray();

                var documento = new Documento
                {
                    Caminhodocumento = file.FileName,
                    Documentonome = file.FileName,
                    Idusuario = idusuario,
                    FileData = fileData
                };

                _context.Documentos.Add(documento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index"); // ou outra ação de sua escolha
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
        // POST: Documentos/SendDocumentToChatGpt
        [HttpPost]
        public async Task<IActionResult> SendDocumentToChatGpt(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null || documento.FileData == null)
            {
                return NotFound();
            }

            // Verifica se o documento é do tipo DOCX
            if (documento.Caminhodocumento.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
            {
                var textoExtraido = ExtrairTextoDeDocx(documento.FileData);
                // Enviar `textoExtraido` para o ChatGPT
                var respostaChatGpt = await EnviarParaChatGpt(textoExtraido);

                // Retorne a resposta do ChatGPT à view ou faça outra ação
                return Content(respostaChatGpt);
            }

            return BadRequest("Formato de documento não suportado.");
        }

        private string ExtrairTextoDeDocx(byte[] fileData)
        {
            using (MemoryStream stream = new MemoryStream(fileData))
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, false))
                {
                    var text = wordDoc.MainDocumentPart.Document.Body
                        .Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>()
                        .Select(t => t.Text)
                        .Aggregate((current, next) => current + next);

                    // Adiciona uma mensagem de depuração para verificar o texto extraído
                    Console.WriteLine("Texto extraído: " + text);

                    return text;
                }
            }
        }


        private async Task<string> EnviarParaChatGpt(string texto)
        {
            var apiKey = "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"; // Substitua pela sua chave de API
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API Key não configurada.");
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                new { role = "system", content = "Você é um assistente de processamento de texto. Por favor, identifique as palavras-chave importantes no texto fornecido abaixo." },
                new { role = "user", content = texto }
            },
                    max_tokens = 150
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    var responseText = responseJson?.choices?[0]?.message?.content;
                    return responseText ?? "Sem resposta";
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    return $"Erro: {errorResponse}";
                }
            }
        }
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public async Task<string> GetChatGptResponse(string documentText)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "Você é um assistente de processamento de texto. Por favor, identifique as palavras-chave importantes no texto fornecido abaixo." },
            new { role = "user", content = documentText }
        },
                max_tokens = 150
            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                // Adiciona uma mensagem de depuração para verificar a resposta
                Console.WriteLine("Resposta da API: " + result);
                var responseJson = JsonConvert.DeserializeObject<dynamic>(result);
                var responseText = responseJson?.choices?[0]?.message?.content;
                return responseText ?? "Sem resposta";
            }

            return "Falha ao obter resposta";
        }


    }

}



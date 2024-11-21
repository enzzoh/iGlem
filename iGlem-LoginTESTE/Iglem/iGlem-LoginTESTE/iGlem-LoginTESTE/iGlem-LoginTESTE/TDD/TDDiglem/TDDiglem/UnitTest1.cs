using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Xunit;
using Microsoft.Data.SqlClient;

public class SuiteTests : IDisposable
{
    public IWebDriver driver { get; private set; }
    public IDictionary<String, Object> vars { get; private set; }
    public IJavaScriptExecutor js { get; private set; }


   // private const string ConnectionString = "Server=localhost;Database=DATAtcc;Trusted_Connection=True;TrustServerCertificate=True;";
    private const string ConnectionString = "Server=localhost;Data Source=.\\SENAI;Initial Catalog=DATAtcc;Persist Security Info=True;User ID=sa;Password=senai.123;TrustServerCertificate=True";
    
    public SuiteTests()
    {
        driver = new ChromeDriver();
        js = (IJavaScriptExecutor)driver;
        vars = new Dictionary<String, Object>();
    }

    public void Dispose()
    {
        driver.Quit();
    }

    [Fact]
    public void CriarLogin()
    {
        driver.Navigate().GoToUrl("https://localhost:7263/");
        driver.Manage().Window.Size = new System.Drawing.Size(945, 1012);

        driver.FindElement(By.LinkText("Inscrever-se")).Click();
        driver.FindElement(By.Name("Nomeusuario")).SendKeys("Jambrolhas");
        driver.FindElement(By.Name("Emailusuario")).SendKeys("Jambrolhas2222@gmail.com");
        driver.FindElement(By.Name("Senhausuario")).SendKeys("123123123");
        driver.FindElement(By.Id("submit-signup")).Click();

        // Aguarda um pouco para que o usuário seja criado no banco de dados
        Thread.Sleep(2000);

        bool userCreated = CheckIfUserExists("Jambrolhas");
        Assert.True(userCreated, "O usuário não foi criado no banco de dados.");
    }

    private bool CheckIfUserExists(string username)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string query = "SELECT COUNT(*) FROM usuario WHERE Nomeusuario = @Nomeusuario";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nomeusuario", username);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
    [Fact]
    public void LOGAR()
    {
       
        driver.Navigate().GoToUrl("https://localhost:7263/");
        driver.Manage().Window.Size = new System.Drawing.Size(945, 1012);

      
        driver.FindElement(By.Name("email")).SendKeys("JambrolhasXD@hotmail.com");
        // driver.FindElement(By.Name("senha")).SendKeys("98765432dd1"); 
        driver.FindElement(By.Name("senha")).SendKeys("123123123");
        driver.FindElement(By.Id("submit-login")).Click();

     
        var isLoggedIn = IsLoginSuccessful();

        Assert.True(isLoggedIn, "Login não realizado com sucesso.");
    }

    private bool IsLoginSuccessful()
    {
        try
        {
       
            var element = driver.FindElement(By.Id("criardocumento"));
            return element.Displayed;
        }
        catch (NoSuchElementException)
        {
           
            return false;
        }
    }







    [Fact]
    public void CriarDocumento()
    {
        // Arrange
        driver.Navigate().GoToUrl("https://localhost:7263/");
        driver.Manage().Window.Size = new System.Drawing.Size(945, 1012);

        // Login
        driver.FindElement(By.Name("email")).SendKeys("JambrolhasXD@Hotmail.com");
        driver.FindElement(By.Name("senha")).SendKeys("123123123");
        driver.FindElement(By.Id("submit-login")).Click();

        // Navigate to create document page
        driver.FindElement(By.Id("criardocumento")).Click();

        // Upload document
        //driver.FindElement(By.Name("file")).SendKeys("C:\\Users\\Piteu\\Downloads\\IglemExcel.xlsx");
        driver.FindElement(By.Name("file")).SendKeys("C:\\Users\\Tarde.Cetafest\\Documents\\pinacocitos.docx");
        driver.FindElement(By.CssSelector(".btn")).Click();

        // Wait for some time to ensure document upload is completed
        Thread.Sleep(5000); 

        // Assert: Verify the document is listed in the index page
        driver.Navigate().GoToUrl("https://localhost:7263/Listadocumentos/homelist");

        // Mensagem de depuração
        var pageSource = driver.PageSource;
        Console.WriteLine(pageSource); 

        var documentExists = IsDocumentPresent("pinacocitos.docx");

        Assert.True(documentExists, "O documento não foi criado com sucesso.");
    }

    private bool IsDocumentPresent(string fileName)
    {
        try
        {
          
            var elements = driver.FindElements(By.XPath("//table//td[contains(text(),'" + fileName + "')]"));
            return elements.Count > 0;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }


}

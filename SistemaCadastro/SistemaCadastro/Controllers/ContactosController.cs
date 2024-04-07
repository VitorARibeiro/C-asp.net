using Microsoft.AspNetCore.Mvc;



namespace SistemaCadastro.Controllers;

public class ContactosController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Adicionar()
    {
        return View();
    }
    
    public IActionResult Apagar()
    {
        return View();
    }
    
    public IActionResult Editar()
    {
        return View();
    }
    
    
}
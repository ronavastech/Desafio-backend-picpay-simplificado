namespace PicPaySimplificado.Services.Autorizador;

public interface IAutorizadorService
{
    Task<bool> AuthorizeAsync();
}
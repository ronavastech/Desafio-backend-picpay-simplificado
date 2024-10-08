using PicPaySimplificado.Models.Requests;
using PicPaySimplificado.Models.Response;

namespace PicPaySimplificado.Services.Carteiras;

public interface ICarteiraService
{
    Task<Result<bool>> ExecuteAsync(CarteiraRequest request);
}

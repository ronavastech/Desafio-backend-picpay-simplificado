using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PicPaySimplificado.Models.Enum;
using PicPaySimplificado.Utils;

namespace PicPaySimplificado.Models.Requests;

public class CarteiraRequest
{
    [Required(ErrorMessage = "O nomeCompleto é obrigatório.")]
    public string NomeCompleto { get; set; }
    
    [Required(ErrorMessage = "O CPF ou CNPJ é obrigatório.")]
    [CpfCnpjValidation(ErrorMessage = "O CPF ou CNPJ informado é inválido.")]
    public string CPFCNPJ { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email deve ser válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserType UserType { get; set; }
    
    [Required(ErrorMessage = "A Saldo em conta é obrigatório.")]
    public decimal SaldoConta { get; set; }
}
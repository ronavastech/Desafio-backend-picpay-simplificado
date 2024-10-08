using System.ComponentModel.DataAnnotations;

namespace PicPaySimplificado.Models.Requests;

public class TransferenciaRequest
{
    [Required(ErrorMessage = "O campo valor é obrigatório.")]
    public decimal Valor { get; set; }
    
    [Required(ErrorMessage = "O campo senderID é obrigatório.")]
    public int SenderId { get; set; }
    
    [Required(ErrorMessage = "O campo reciverId é obrigatório.")]
    public int ReciverId { get; set; }
}
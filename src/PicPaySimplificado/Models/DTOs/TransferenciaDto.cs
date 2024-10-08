namespace PicPaySimplificado.Models.DTOs;

public record TransferenciaDto(Guid IdTransaction, CarteiraEntity Sender, CarteiraEntity Reciver, decimal ValorTransferido);

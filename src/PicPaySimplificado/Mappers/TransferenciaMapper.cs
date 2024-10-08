using PicPaySimplificado.Models;
using PicPaySimplificado.Models.DTOs;

namespace PicPaySimplificado.Mappers;

public static class TransferenciaMapper
{
    public static TransferenciaDto ToTransferenciaDto(this TransferenciaEntity transaction)
    {
        return new TransferenciaDto(
            transaction.IdTransferencia, 
            transaction.Sender, 
            transaction.Reciver, 
            transaction.Valor
        );
    }
}
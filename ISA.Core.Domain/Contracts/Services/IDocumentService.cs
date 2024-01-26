namespace ISA.Core.Domain.Contracts.Services
{
    using ceTe.DynamicPDF;
    using ISA.Core.Domain.Entities.Reservation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IDocumentService
    {
        Document GeneratePdf(List<ReservationEquipment> reservationEquipment);
    }
}

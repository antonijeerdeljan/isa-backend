namespace ISA.Core.Domain.UseCases.Reservation
{
    using ISA.Core.Domain.Contracts.Repositories;
    using ceTe.DynamicPDF;
    using ceTe.DynamicPDF.PageElements;
    using ceTe.DynamicPDF.PageElements.BarCoding;
    using ISA.Core.Domain.Entities.Reservation;
    using ISA.Core.Domain.Contracts.Services;

    public class DocumentService : IDocumentService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IReservationEquipmentRepository _reservationEquipmentRepository;

        public DocumentService(IReservationRepository reservationRepository, ICustomerRepository customerRepository, IAppointmentRepository appointmentRepository, IReservationEquipmentRepository reservationEquipmentRepository)
        {
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
            _appointmentRepository = appointmentRepository;
            _reservationEquipmentRepository = reservationEquipmentRepository;
        }

        public Document GeneratePdf(List<ReservationEquipment> reservationEquipment)
        {
            Document document = new Document();
            Page page = new Page(PageSize.A4, PageOrientation.Portrait, 54.0f);
            //document.Pages.Add(page);
            String content = new string("Postovani Marko" + "" + ",\nOvdje mozete da vidite sve stavke vase narudzbe kao i njihovu kolicinu. Ispod se nalazi i qr kod sa kojim mozete preuzeti narudzbu.");
            Label label = new Label("Potvrda porudzbine", 0, 0, 504, 100, Font.Helvetica, 30.0f, TextAlign.Center);
            Label header = new Label(content, 0, 60, 504, 100, Font.Helvetica, 12.0f, TextAlign.Left);
            page.Elements.Add(label);
            page.Elements.Add(header);
            Table2 table = new Table2(50, 120, 500, reservationEquipment.Count() * 30);
            table.Columns.Add(250); // Adjust the width based on your needs
            table.Columns.Add(150);  // Adjust the width based on your needs
            Row2 headerRow = table.Rows.Add();
            headerRow.Cells.Add("Naziv proizvoda");
            headerRow.Cells.Add("Kolicina");

            foreach (var equipment in reservationEquipment)
            {
                Row2 dataRow = table.Rows.Add();
                dataRow.Cells.Add(equipment.Equipment.Name).Align = (TextAlign?)Align.Center;
                dataRow.Cells.Add(equipment.Quantity.ToString()).Align = (TextAlign?)Align.Center;
            }

            page.Elements.Add(table);

            // Add the page to the document
            document.Pages.Add(page);
            document.Draw("C:\\Users\\I\\Desktop\\ISA projekat\\File.pdf");

            return document;
        }

    }
}

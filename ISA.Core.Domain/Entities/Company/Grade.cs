namespace ISA.Core.Domain.Entities.Company
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Grade : Entity<Guid> 
    {
        public Guid CustomerUserId {  get; set; }
        public Guid CompanyId {  get; set; }
        public int Rate {  get; set; }
        public string Reason {  get; set; }
        public string Text { get; set; }

        public Grade()
        {
        }

        public Grade(Guid userId, Guid companyId, int rate, string reason, string text)
        {
            CustomerUserId = userId;
            CompanyId = companyId;
            Rate = rate;
            Reason = reason;
            Text = text;
        }

        public Grade(Guid id, Guid userId, Guid companyId, int rate, string reason, string text)
        {
            Id = id;
            CustomerUserId = userId;
            CompanyId = companyId;
            Rate = rate;
            Reason = reason;
            Text = text;
        }
    }
}

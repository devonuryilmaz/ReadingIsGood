using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ReadingIsGood.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int ID { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [JsonIgnore]
        public virtual User? CreatedUser { get; set; }

        [ForeignKey(nameof(ModifiedBy))]
        [JsonIgnore]
        public virtual User? ModifiedUser { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCutAndGoApp.Models {

    public enum AdminRole :byte { Propietario = 1, Gerente = 2, Supervisor = 3, Empleado = 4 }

    [Table("ADMINS")]
    public class Admin {
        [Column("hairdresser_id")]
        public int HairdresserId { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("role")]
        public AdminRole Role { get; set; }

        [ForeignKey(nameof(HairdresserId))]
        public virtual Hairdresser Hairdresser { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }

}

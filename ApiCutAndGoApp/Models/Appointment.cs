using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCutAndGoApp.Models {

    public enum StatusAppointment :int { NoConfirmada = 0, Finalizada = 1, Activa = 2, Cancelada = 3 }

    [Table("APPOINTMENTS")]
    public class Appointment {

        [Key] [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("hairdresser_id")]
        public int HairdresserId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("time")]
        public TimeSpan Time { get; set; }

        [Column("status")]
        public StatusAppointment Status { get; set; }

    }

}

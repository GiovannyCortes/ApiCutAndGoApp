using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCutAndGoApp.Models {

    [Table("APPOINTMENT_SERVICES")]
    public class Appointment_Service {
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [Column("service_id")]
        public int ServiceId { get; set; }
    }

}

using ApiCutAndGoApp.Data;
using CutAndGo.Interfaces;
using CutAndGo.Models;
using CutAndGo.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static CutAndGo.Interfaces.IRepositoryHairdresser;

namespace ApiCutAndGoApp.Repositores {
    public class RepositoryHairdresser : IRepositoryHairdresser {

        HairdressersContext context;

        public RepositoryHairdresser(HairdressersContext context) {
            this.context = context;
        }

        #region CREDENTIALS
        public async Task<User?> LogInAsync(string email, string password) {
            User? user = await this.context.Users.FirstOrDefaultAsync(u => u.Email == email);
            bool passwordConfirmed = false;
            if (user != null) {
                byte[] passUser = user.Password;
                byte[] passTemp = HelperCryptography.EncryptContent(password, user.Salt);
                passwordConfirmed = HelperCryptography.CompareArrays(passUser, passTemp);
            }
            return (passwordConfirmed)? user : null;
        }
        #endregion

        #region TOKENS
        public string GenerateToken() {
            const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var token = new string(
                    Enumerable.Repeat(caracteresPermitidos, 50).Select(s => s[random.Next(s.Length)]).ToArray()
                );
            return token;
        }

        public async Task<bool> UserAssignTokenAsync(int user_id, string token) {
            User? user = await this.FindUserAsync(user_id);
            if (user != null) {
                user.TempToken = token;
                return true;
            }
            return false;
        }

        public async Task<bool> UserValidateTokenAsync(int user_id, string token) {
            return await this.context.Users.AnyAsync(u => u.UserId == user_id && u.TempToken == token);
        }

        public async Task<bool> HairdresserValidateTokenAsync(int hairdresser_id, string token) {
            return await this.context.Hairdressers.AnyAsync(h => h.HairdresserId == hairdresser_id && h.Token == token);
        }
        #endregion

        #region ADMINS
        public async Task<Admin?> FindAdminAsync(int hairdresser_id, int user_id) {
            return await this.context.Admins.FirstOrDefaultAsync(a => a.HairdresserId == hairdresser_id && a.UserId == user_id);
        }

        public async Task<List<Admin>> GetAdminsAsync(int hairdresser_id) {
            return await this.context.Admins.Where(a => a.HairdresserId == hairdresser_id).ToListAsync();
        }

        public async Task<bool> AdminExistAsync(int hairdresser_id, int user_id) {
            return await context.Admins.AnyAsync(admin => admin.UserId == user_id && admin.HairdresserId == hairdresser_id);
        }

        public async Task<bool> CompareAdminRoleAsync(int hairdresser_id, int user_id_me, int user_id_other) {
            Admin? userMe = await this.context.Admins.FirstOrDefaultAsync(a => a.UserId == user_id_me);
            Admin? userOther = await this.context.Admins.FirstOrDefaultAsync(a => a.UserId == user_id_other);
            bool response = false;
            if (userMe != null && userOther != null) {
                response = (userMe.Role <= userOther.Role) ? true : false; 
            }
            return (userMe == null || userOther == null) ? false : response;
        }

        public async Task<Response> InsertAdminAsync(int hairdresser_id, int user_id, AdminRole role) {
            bool exist = await AdminExistAsync(hairdresser_id, user_id);
            if (!exist) {
                Admin new_admin = new Admin {
                    HairdresserId = hairdresser_id,
                    UserId = user_id,
                    Role = role
                };
                this.context.Admins.Add(new_admin);
                await this.context.SaveChangesAsync();
                return new Response { ResponseCode = (int)ResponseCodes.OK };
            } else {
                return new Response {
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.Duplicate,
                    ErrorMessage = "Este usuario ya es administrador de la peluquería señalada"
                };
            }; 
        }

        public async Task<Response> UpdateAdminAsync(int hairdresser_id, int user_id, AdminRole role) {
            Admin? admin = await this.FindAdminAsync(hairdresser_id, user_id);
            if (admin != null) {
                admin.Role = role;
                await context.SaveChangesAsync();
                return new Response { ResponseCode = (int)ResponseCodes.OK, SatisfactoryId = user_id };
            } else {
                return new Response { 
                    ResponseCode = (int)ResponseCodes.Failed, 
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound,
                    ErrorMessage = "Administrador no encontrado"
                };
            }
        }

        public async Task<Response> DeleteAdminAsync(int hairdresser_id, int user_id_me, int user_id_other) {
            if (await this.CompareAdminRoleAsync(hairdresser_id, user_id_me, user_id_other)) {
                Admin? admin = await FindAdminAsync(hairdresser_id, user_id_other);
                if (admin != null) {
                    context.Admins.Remove(admin);
                    await context.SaveChangesAsync();
                    return new Response { ResponseCode = (int)ResponseCodes.OK };
                }
                return new Response {
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound,
                    ErrorMessage = "Administrador no encontrado"
                };
            } else {
                return new Response { ResponseCode = (int)ResponseCodes.NotAuthorized };
            }
        }
        #endregion

        #region USERS
        public async Task<bool> UserIsAdminAsync(int user_id) {
            return await this.context.Admins.AnyAsync(a => a.UserId == user_id);
        }

        public async Task<bool> EmailExistAscync(string email) {
            return await this.context.Users.AnyAsync(a => a.Email == email);
        }

        public async Task<User?> FindUserAsync(int user_id) {
            return await this.context.Users.FirstOrDefaultAsync(u => u.UserId == user_id);
        }

        public async Task<User?> InsertUserAsync(string password, string name, string lastname, string phone, string email, bool econfirmed) {
            var newid = await this.context.Users.AnyAsync() ? context.Users.Max(u => u.UserId) + 1 : 1;
            string salt = HelperCryptography.GenerateSalt();

            User user = new User {
                UserId = newid,
                Salt = salt,
                Password = HelperCryptography.EncryptContent(password, salt),
                PasswordRead = password,
                Name = name,
                LastName = lastname,
                Phone = phone,
                Email = email,
                EmailConfirmed = econfirmed,
                TempToken = ""
            };

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task<Response> UpdateUserAsync(int user_id, string name, string lastname, string phone, string email) {
            User? user = await this.FindUserAsync(user_id);
            if (user != null) {
                user.Name = name;
                user.LastName = lastname;
                user.Phone = phone;
                user.Email = email;
                await this.context.SaveChangesAsync();
                return new Response { ResponseCode = (int)ResponseCodes.OK };
            } else {
                return new Response { 
                    ResponseCode = (int)ResponseCodes.Failed, 
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound, 
                    ErrorMessage = "Usuario no encontrado" 
                };
            }
        }

        public async Task<Response> ValidateEmailAsync(int user_id) {
            User? user = await this.FindUserAsync(user_id);
            if (user != null) {
                user.EmailConfirmed = true;
                await this.context.SaveChangesAsync();
                return new Response { ResponseCode = (int)ResponseCodes.OK };
            } else {
                return new Response {
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound,
                    ErrorMessage = "Usuario no encontrado"
                };
            }
        }
        #endregion

        #region HAIRDRESSERS
        public async Task<Hairdresser?> FindHairdresserAsync(int hairdresser_id) {
            return await this.context.Hairdressers.FirstOrDefaultAsync(h => h.HairdresserId == hairdresser_id);
        }

        public async Task<List<string>> GetHairdresserEmailsAsync(int hairdresser_id) {
            var emails = from usuario in context.Users
                         join admin in context.Admins on usuario.UserId equals admin.UserId
                         where admin.HairdresserId == hairdresser_id
                         select usuario.Email;
            return await emails.ToListAsync();
        }

        public async Task<List<Hairdresser>> GetHairdressersAsync() {
            return await this.context.Hairdressers.ToListAsync();
        }

        public async Task<List<Hairdresser>> GetHairdressersByUserAsync(int user_id) {
            var query = this.context.Hairdressers
                        .Join(
                            context.Admins,
                            hairdresser => hairdresser.HairdresserId,
                            admin => admin.HairdresserId,
                            (hairdresser, admin) => new { Hairdresser = hairdresser, Admin = admin }
                        )
                        .Join(
                            context.Users,
                            admin => admin.Admin.UserId,
                            user => user.UserId,
                            (admin, user) => new { admin.Hairdresser, User = user }
                        )
                        .Where(x => x.User.UserId == user_id).Select(x => x.Hairdresser);
            return await query.ToListAsync();
        }

        public async Task<List<Hairdresser>> GetHairdressersByFilterNameAsync(string filterName) {
            var query = from data in this.context.Hairdressers
                        where data.Name.ToLower().Contains(filterName.ToLower())
                        select data;
            return await query.ToListAsync();
        }

        public async Task<Response> InsertHairdresserAsync(string name, string phone, string address, int postal_code, int user_id) {
            var newid = await this.context.Hairdressers.AnyAsync() ? context.Hairdressers.Max(s => s.HairdresserId) + 1 : 1;
            Hairdresser hairdresser = new Hairdresser {
                HairdresserId = newid,
                Name = name,
                Phone = phone,
                Address = address,
                PostalCode = postal_code,
                Token = GenerateToken()
            };
            this.context.Hairdressers.Add(hairdresser);
            await this.context.SaveChangesAsync();
            await this.InsertAdminAsync(newid, user_id, AdminRole.Propietario);
            return new Response { ResponseCode = (int)ResponseCodes.OK, SatisfactoryId = newid };
        }

        public async Task<Response> UpdateHairdresserAsync(int hairdresser_id, string name, string phone, string address, int postal_code) {
            Hairdresser? hairdresser = await FindHairdresserAsync(hairdresser_id);
            if (hairdresser != null) {
                hairdresser.Name = name;
                hairdresser.Phone = phone;
                hairdresser.Address = address;
                hairdresser.PostalCode = postal_code;

                await this.context.SaveChangesAsync();
                return new Response { ResponseCode = (int)ResponseCodes.OK, SatisfactoryId = hairdresser_id };
            } else {
                return new Response {
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound,
                    ErrorMessage = "Peluquería no encontrada"
                };
            }
        }

        public async Task<Response> DeleteHairdresserAsync(int hairdresser_id) {
            Hairdresser? hairdresser = await this.FindHairdresserAsync(hairdresser_id);
            if (hairdresser != null) {
                /// 1 - Borrar los registros de Admin
                List<Admin> admins = await this.GetAdminsAsync(hairdresser_id);
                foreach (Admin admin in admins) {
                    this.context.Admins.Remove(admin);
                }
                await this.context.SaveChangesAsync();

                /// 2 - Borrar los registros de Horario
                List<Schedule> schedules = await this.GetSchedulesAsync(hairdresser_id, true);
                foreach (Schedule schedule in schedules) {
                    foreach (Schedule_Row schedule_Row in schedule.ScheduleRows) {
                        this.context.Schedule_Rows.Remove(schedule_Row);
                    }
                    await this.context.SaveChangesAsync();
                    this.context.Schedules.Remove(schedule);
                }
                await this.context.SaveChangesAsync();

                /// 3 - Borrar los registros de citas y sus relaciones con servicios
                List<Appointment> appointments = await this.GetAppointmentsByHairdresserAsync(hairdresser_id);
                foreach (Appointment appointment in appointments) {
                    List<Appointment_Service> appointment_Services = await this.GetAppointmentServicesAsync(appointment.AppointmentId);
                    foreach (Appointment_Service appointment_Service in appointment_Services) {
                        this.context.AppointmentServices.Remove(appointment_Service);
                    }
                    await this.context.SaveChangesAsync();
                    this.context.Appointments.Remove(appointment);
                }
                await this.context.SaveChangesAsync();

                /// 4 - Borrar los registros de servicios
                List<Service> services = await this.GetServicesByHairdresserAsync(hairdresser_id);
                foreach (Service service in services) {
                    this.context.Services.Remove(service);
                }
                await this.context.SaveChangesAsync();

                /// 5 - Eliminar la peluquería
                this.context.Hairdressers.Remove(hairdresser);
                await this.context.SaveChangesAsync();

                return (await this.FindHairdresserAsync(hairdresser_id) == null) ? 
                    new Response { ResponseCode = (int)ResponseCodes.OK } : 
                    new Response { 
                        ResponseCode = (int)ResponseCodes.Failed,
                        ErrorCode = (int)ResponseErrorCodes.GeneralError,
                        ErrorMessage = "Error inesperado. La peluquería no ha podido ser eliminada"
                    };
            } else {
                return new Response { 
                    ResponseCode = (int)ResponseCodes.Failed, 
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound,
                    ErrorMessage = "Peluquería no encontrada"
                };
            }
        }
        #endregion

        #region SCHEDULES
        public async Task<Schedule?> FindScheduleAsync(int schedule_id, bool getRows) {
            Schedule? schedule = await this.context.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == schedule_id);
            if (schedule != null && getRows) {
                List<Schedule_Row> schedule_rows = await this.GetScheduleRowsAsync(schedule.ScheduleId);
                foreach (Schedule_Row row in schedule_rows) {
                    schedule.ScheduleRows.Add(row);
                }
            }
            return schedule;
        }

        public async Task<Schedule?> FindActiveScheduleAsync(int hairdresser_id, bool getRows) {
            return await this.context.Schedules.FirstOrDefaultAsync(s => s.HairdresserId == hairdresser_id && s.Active == true);
        }

        public async Task<List<string>> GetNameSchedulesAsync(int hairdresser_id) {
            var query = from data in context.Schedules
                           where data.HairdresserId == hairdresser_id
                           select data.Name;
            return await query.ToListAsync();
        }

        public async Task<List<Schedule>> GetSchedulesAsync(int hairdresser_id, bool getRows) {
            List<Schedule> schedules = await this.context.Schedules.Where(s => s.HairdresserId == hairdresser_id).ToListAsync();
            if (getRows) {
                foreach (Schedule sch in schedules) {
                    List<Schedule_Row> schedule_rows = await this.GetScheduleRowsAsync(sch.ScheduleId);
                    foreach (Schedule_Row row in schedule_rows) {
                        sch.ScheduleRows.Add(row);
                    }
                }
            }
            return schedules;
        }

        public async Task<Response> InsertScheduleAsync(int hairdresser_id, string name, bool active) {
            List<Schedule> schedules = await this.GetSchedulesAsync(hairdresser_id, false);
            bool duplicado = false;
            foreach (Schedule sch in schedules) {
                if (sch.Name.ToLower() == name.ToLower()) { duplicado = true; }
            }

            if (!duplicado) {
                if (active) { // Se pretende activar el nuevo horario
                    foreach (Schedule sch in schedules) {
                        if (sch.Active) { sch.Active = false; }
                    }
                }

                var newid = await this.context.Schedules.AnyAsync() ? this.context.Schedules.Max(s => s.ScheduleId) + 1 : 1;
                Schedule schedule = new Schedule {
                    ScheduleId = newid,
                    HairdresserId = hairdresser_id,
                    Name = name,
                    Active = active
                };

                this.context.Schedules.Add(schedule);
                await this.context.SaveChangesAsync();
                return new Response { ResponseCode = (int)ResponseCodes.OK };
            } else {
                return new Response { 
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.Duplicate,
                    ErrorMessage = "Ya existe un horario con el mismo nombre"
                };
            }
        }

        public async Task<Response> UpdateScheduleAsync(int schedule_id, int hairdresser_id, string name, bool active) {
            Schedule? schedule = await this.FindScheduleAsync(schedule_id, false);
            if (schedule != null) {
                schedule.Name = name;
                schedule.Active = active;

                if (active) {
                    List<Schedule> schedules = await GetSchedulesAsync(hairdresser_id, false);
                    foreach (Schedule sch in schedules) {
                        if (sch.ScheduleId != schedule_id && sch.Active) { sch.Active = false; }
                    }
                }

                await this.context.SaveChangesAsync();
                return new Response { ResponseCode = (int)ResponseCodes.OK, SatisfactoryId = schedule_id };
            } else {
                return new Response {
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound,
                    ErrorMessage = "Horario no encontrado"
                };
            }
        }

        public async Task<Response> DeleteScheduleAsync(int schedule_id) {
            Schedule? schedule = await this.FindScheduleAsync(schedule_id, true);
            if (schedule != null) {
                List<Schedule> schedules = await GetSchedulesAsync(schedule.HairdresserId, false);
                if (schedules.Count > 1) {
                    if (schedule.ScheduleRows != null && schedule.ScheduleRows.Count > 0) {
                        foreach (Schedule_Row srow in schedule.ScheduleRows) {
                            this.context.Schedule_Rows.Remove(srow);
                            await this.context.SaveChangesAsync();
                        }
                    }
                    this.context.Schedules.Remove(schedule);
                    await this.context.SaveChangesAsync();
                    return new Response { ResponseCode = (int)ResponseCodes.OK };
                } else {
                    return new Response {
                        ResponseCode = (int)ResponseCodes.Failed,
                        ErrorCode = (int)ResponseErrorCodes.GeneralError,
                        ErrorMessage = "No puede haber peluquerías sin horario. Este es el último horario de la peluquería"
                    };
                }
            } else {
                return new Response {
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound,
                    ErrorMessage = "Horario no encontrado"
                };
            }
        }

        public async Task<Response> ActivateScheduleAsync(int hairdresser_id, int schedule_id) {
            Schedule? schedule = await FindScheduleAsync(schedule_id, false);
            if (schedule != null) {
                List<Schedule> schedules = await GetSchedulesAsync(hairdresser_id, false);
                foreach (Schedule sch in schedules) {
                    if (sch.Active) { sch.Active = false; }
                }
                schedule.Active = true;
                await this.context.SaveChangesAsync();
                return new Response { ResponseCode = (int)ResponseCodes.OK, SatisfactoryId = schedule_id };
            } else {
                return new Response {
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.RecordNotFound,
                    ErrorMessage = "Horario no encontrado"
                };
            }
        }
        #endregion

        #region SCHEDULE_ROWS
        public async Task<Schedule_Row?> FindScheduleRowAsync(int schedule_row_id) {
            return await this.context.Schedule_Rows.FirstOrDefaultAsync(s => s.ScheduleRowId == schedule_row_id);
        }

        public async Task<List<Schedule_Row>> GetScheduleRowsAsync(int schedule_id) {
            return await this.context.Schedule_Rows.Where(s => s.ScheduleId == schedule_id).ToListAsync();
        }

        public async Task<List<Schedule_Row>> GetActiveScheduleRowsAsync(int hairdresser_id) {
            Schedule? schedule = await this.FindActiveScheduleAsync(hairdresser_id, false);
            if (schedule != null) {
                return await this.context.Schedule_Rows.Where(s => s.ScheduleId == schedule.ScheduleId).ToListAsync();
            } else {
                return new List<Schedule_Row>();
            }
        }

        public async Task<Response> InsertScheduleRowsAsync(int schid, TimeSpan start, TimeSpan end, bool mon, bool tue, bool wed, bool thu, bool fri, bool sat, bool sun) {
            throw new NotImplementedException();
        }

        public async Task<Response> ValidateScheduleRowAsync(Schedule_Row schedule_row) {
            int schrow_compare = TimeSpan.Compare(schedule_row.Start, schedule_row.End);
            if (schrow_compare == 0 || schrow_compare == 1) { // Validación de rango
                return new Response { 
                    ResponseCode = (int)ResponseCodes.Failed,
                    ErrorCode = (int)ResponseErrorCodes.IncorrectRange,
                    ErrorMessage = "El inicio y el final del registro no conforman un rango válido"
                }; 
            }

            List<Schedule_Row> rows = await GetScheduleRowsAsync(schedule_row.ScheduleId);
            foreach (Schedule_Row row in rows) {
                if (
                    row.ScheduleRowId != schedule_row.ScheduleRowId &&
                    row.ScheduleId == schedule_row.ScheduleId &&
                    row.Start == schedule_row.Start &&
                    row.End == schedule_row.End &&
                    row.Monday == schedule_row.Monday &&
                    row.Tuesday == schedule_row.Tuesday &&
                    row.Wednesday == schedule_row.Wednesday &&
                    row.Thursday == schedule_row.Thursday &&
                    row.Friday == schedule_row.Friday &&
                    row.Saturday == schedule_row.Saturday &&
                    row.Sunday == schedule_row.Sunday
                ) {
                    return new Response {
                        ResponseCode = (int)ResponseCodes.Failed,
                        ErrorCode = (int)ResponseErrorCodes.Duplicate,
                        ErrorMessage = "Ya existe un registro con las mismas características"
                    };
                }

                if (
                    row.Monday & row.Monday == schedule_row.Monday |
                    row.Tuesday & row.Tuesday == schedule_row.Tuesday |
                    row.Wednesday & row.Wednesday == schedule_row.Wednesday |
                    row.Thursday & row.Thursday == schedule_row.Thursday |
                    row.Friday & row.Friday == schedule_row.Friday |
                    row.Saturday & row.Saturday == schedule_row.Saturday |
                    row.Sunday & row.Sunday == schedule_row.Sunday
                ) { // SOLO COMPROBAREMOS EL SOLAPAMIENTO DE TIEMPOS SI HAY COMO MÍNIMO UN DÍA COINCIDENTE
                    int start_compare_start = TimeSpan.Compare(schedule_row.Start, row.Start);
                    int start_compare_end = TimeSpan.Compare(schedule_row.Start, row.End);
                    int end_compare_start = TimeSpan.Compare(schedule_row.End, row.Start);
                    int end_compare_end = TimeSpan.Compare(schedule_row.End, row.End);

                    if (
                        row.ScheduleRowId != schedule_row.ScheduleRowId && (
                            start_compare_start == 0 ||                                                     // Los inicios igual al rango
                            start_compare_start == 1 && start_compare_end == -1 ||                          // El inicio está entre los rangos
                            start_compare_start == 1 && (end_compare_end == 0 || end_compare_end == -1) ||  // Inicio correcto, final menor o igual al rango
                            start_compare_start == -1 && end_compare_start == 1 && end_compare_end == -1 || // Inicio correcto, final entre rangos
                            start_compare_start == -1 && (end_compare_end == 0 || end_compare_end == 1)     // Inicio correcto, final superior al rango
                        )
                    ) {
                        return new Response {
                            ResponseCode = (int)ResponseCodes.Failed,
                            ErrorCode = (int)ResponseErrorCodes.OverwriteRange,
                            ErrorMessage = "Rango sobreescrito. Se ha producido un solapamiento"
                        };
                    }
                }

            }
            return new Response { ResponseCode = (int)ResponseCodes.OK };
        }

        public async Task<Response> DeleteScheduleRowsAsync(int schedule_row_id) {
            throw new NotImplementedException();
        }
        #endregion

        #region APPOINTMENTS
        public Task<Appointment?> FindAppoinmentAsync(int appointment_id) {
            throw new NotImplementedException();
        }

        public Task<List<Appointment>> GetAppointmentsByUserAsync(int user_id) {
            throw new NotImplementedException();
        }

        public Task<List<Appointment>> GetAppointmentsByHairdresserAsync(int hairdresser_id) {
            throw new NotImplementedException();
        }

        public Task<Response> InsertAppointmentAsync(int user_id, int hairdresser_id, DateTime date, TimeSpan time) {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAppointmentAsync(int appointment_id, DateTime date, TimeSpan time) {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAppointmentAsync(int appointment_id) {
            throw new NotImplementedException();
        }

       public Task<Response> ApproveAppointmentAsync(int appointment_id) {
            throw new NotImplementedException();
        }
        #endregion

        #region SERVICES
        public Task<Service?> FindServiceAsync(int service_id) {
            throw new NotImplementedException();
        }

        public Task<List<Service>> GetServicesByAppointmentAsync(int appoinment_id) {
            throw new NotImplementedException();
        }

        public Task<List<Service>> GetServicesByHairdresserAsync(int hairdresser_id) {
            throw new NotImplementedException();
        }

        public Task<List<Service>> GetServicesByIdentificationAsync(List<int> app_services) {
            throw new NotImplementedException();
        }

        public Task<Response> InsertServiceAsync(int hairdresser_id, string name, decimal price, byte duration) {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateServiceAsync(int service_id, string name, decimal price, byte duration) {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteServiceAsync(int service_id) {
            throw new NotImplementedException();
        }
        #endregion

        #region APPOINTMENT_SERVICES
        public Task<Appointment_Service?> FindAppointmentServiceAsync(int appointment_id, int service_id) {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetAppointmentServicesIDsAsync(int appointment_id) {
            throw new NotImplementedException();
        }

        public Task<List<Appointment_Service>> GetAppointmentServicesAsync(int appointment_id) {
            throw new NotImplementedException();
        }

        public Task<Response> InsertAppointmentServiceAsync(int appointment_id, int service_id) {
            throw new NotImplementedException();
        }

        public Task<Response> DeleteAppointmentServiceAsync(int appointment_id, int service_id) {
            throw new NotImplementedException();
        }
        #endregion

    }
}

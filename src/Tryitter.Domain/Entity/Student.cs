using Flunt.Notifications;
using Flunt.Validations;

namespace Tryitter.Domain.Entity
{
    public class Student : Notifiable<Notification>
    {
        public  Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Module { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        
        public Student() {}
        
        public Student(string name, string email, string module, string status, string password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Module = module;
            Status = status;
            Password = password;
            
            Validate();
        }
        
        public Student(Guid id, string name, string email, string module, string status, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Module = module;
            Status = status;
            Password = password;
            
            Validate();
        }

        private void Validate()
        {
            var contract = new Contract<Student>()
                .IsNotNullOrEmpty(Name, "Name")
                .IsEmail(Email, "Email")
                .IsNotNullOrEmpty(Module, "Module")
                .IsNotNullOrEmpty(Status, "Status")
                .IsNotNullOrEmpty(Password, "Password")
                .IsGreaterThan(Password, 4, "Password");
            AddNotifications(contract);
        }

        public void EditInfo(string name, string module, string status, string password)
        {
            Name = name;
            Module = module;
            Status = status;
            Password = password;
            
            Validate();
        }
    }
}
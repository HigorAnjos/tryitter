using Flunt.Notifications;
using Flunt.Validations;

namespace Tryitter.Domain.Entity
{
    public class Post : Notifiable<Notification>
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        
        public Post() {}
        public Post(string message)
        {
            Id = Guid.NewGuid();
            Message = message;
            
            Validate();
        }
        
        public void EditInfo(Guid id, string message)
        {
            Id = id;
            Message = message;
            
            Validate();
        }

        private void Validate()
        {
            var contract = new Contract<Post>()
                .IsNotNullOrEmpty(Message, "Message")
                .IsGreaterThan(Message, 10, "Message");
            AddNotifications(contract);
        }
    }
}
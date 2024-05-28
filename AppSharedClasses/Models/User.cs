namespace AppSharedClasses.Models
{
    public enum UserType {
        Elementary,
        Experienced,
        Advanced
    }

    public enum Cases {
        Shape,
        Images
    }

    public class User {
        public User(string id, string name, uint age, UserType userType)
        {
            ExperimentId = id;
            Name = name;
            Age = age;
            UserType = userType;
        }

        public string ExperimentId { get; private set; }
        public string Name { get; private set; }
        public uint Age { get; private set; }
        public UserType UserType { get; private set; }
    }
}
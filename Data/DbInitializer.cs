using DaycareManager.Models;

namespace DaycareManager.Data;

public static class DbInitializer
{
    public static void Initialize(DaycareDbContext context)
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Look for any classrooms.
        if (context.Classrooms.Any())
        {
            return;   // DB has been seeded
        }

        var classrooms = new Classroom[]
        {
            new Classroom { ClassroomNumber = "Infant 1", MinAgeMonths = 0, MaxAgeMonths = 12, Capacity = 10 },
            new Classroom { ClassroomNumber = "Toddler 1", MinAgeMonths = 13, MaxAgeMonths = 24, Capacity = 12 },
            new Classroom { ClassroomNumber = "Toddler 2", MinAgeMonths = 25, MaxAgeMonths = 36, Capacity = 12 },
            new Classroom { ClassroomNumber = "Pre-K 1", MinAgeMonths = 37, MaxAgeMonths = 48, Capacity = 15 },
            new Classroom { ClassroomNumber = "Pre-K 2", MinAgeMonths = 49, MaxAgeMonths = 60, Capacity = 15 }
        };
        context.Classrooms.AddRange(classrooms);
        context.SaveChanges();

        // Generate Families (Couples)
        // We need enough families for 50 children.
        // 10 families * 2 children = 20 children
        // 30 families * 1 child = 30 children
        // Total 40 families (80 parents)
        
        var families = new List<List<Parent>>();
        var parentLastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores" };
        var parentFirstNamesMale = new[] { "James", "Robert", "John", "Michael", "David", "William", "Richard", "Joseph", "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony", "Mark", "Donald", "Steven", "Paul", "Andrew", "Joshua" };
        var parentFirstNamesFemale = new[] { "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen", "Lisa", "Nancy", "Betty", "Margaret", "Sandra", "Ashley", "Kimberly", "Emily", "Donna", "Michelle" };

        var random = new Random();

        for (int i = 0; i < 40; i++)
        {
            string lastName = parentLastNames[i % parentLastNames.Length]; // Ensure unique last names if possible or rotate
            
            // Create Couple
            var p1 = new Parent 
            { 
                FirstName = parentFirstNamesMale[random.Next(parentFirstNamesMale.Length)], 
                LastName = lastName, 
                PhoneNumber = "555-01" + i.ToString("D2"), 
                Email = $"parent{i}_1@example.com" 
            };
            var p2 = new Parent 
            { 
                FirstName = parentFirstNamesFemale[random.Next(parentFirstNamesFemale.Length)], 
                LastName = lastName, 
                PhoneNumber = "555-02" + i.ToString("D2"), 
                Email = $"parent{i}_2@example.com" 
            };
            
            families.Add(new List<Parent> { p1, p2 });
        }
        
        // Save parents first to need Ids? No, we can add to context.
        var allParents = families.SelectMany(f => f).ToList();
        context.Parents.AddRange(allParents);
        context.SaveChanges();

        var children = new List<Child>();

        // Distribute children to families
        // First 10 families get 2 children
        // Next 30 families get 1 child
        
        int familyIndex = 0;
        int childrenCount = 0;

        // Bucket definitions for age distribution
        // We need 10 per age group (Total 50)
        // Groups: 0:Infant, 1:Tod1, 2:Tod2, 3:PreK1, 4:PreK2
        var ageGroups = new int[5] { 10, 10, 10, 10, 10 };
        var currentAgeGroup = 0;

        while (childrenCount < 50)
        {
            // Determine how many kids for this family
            int kidsForFamily = (familyIndex < 10) ? 2 : 1;

            var familyParents = families[familyIndex];
            string familyLastName = familyParents[0].LastName;

            for (int k = 0; k < kidsForFamily; k++)
            {
                // Find an available age group
                while (ageGroups[currentAgeGroup] == 0)
                {
                    currentAgeGroup = (currentAgeGroup + 1) % 5;
                }

                // Generate child for this group
                int minAge = 0, maxAge = 0;
                switch (currentAgeGroup)
                {
                    case 0: minAge = 2; maxAge = 11; break;
                    case 1: minAge = 12; maxAge = 23; break;
                    case 2: minAge = 25; maxAge = 35; break;
                    case 3: minAge = 37; maxAge = 47; break;
                    case 4: minAge = 48; maxAge = 59; break;
                }

                var child = CreateChild(minAge, maxAge, familyLastName, familyParents, classrooms, random);
                children.Add(child);
                
                ageGroups[currentAgeGroup]--;
                childrenCount++;
                
                // Rotate group for valid distribution mix
                currentAgeGroup = (currentAgeGroup + 1) % 5; 
            }
            familyIndex++;
        }

        context.Children.AddRange(children);
        context.SaveChanges();
    }

    private static Child CreateChild(int minAgeMonths, int maxAgeMonths, string lastName, List<Parent> parents, Classroom[] classrooms, Random random)
    {
        // Name pools based on criteria
        // Infant (0-12m): A-E
        var namesAE_Male = new[] { "Aaron", "Benjamin", "Charlie", "David", "Ethan", "Austin", "Adam", "Blake", "Caleb", "Daniel", "Evan", "Andrew", "Anthony", "Alexander", "Asher" };
        var namesAE_Female = new[] { "Alice", "Bella", "Chloe", "Daisy", "Emma", "Eva", "Amelia", "Ava", "Charlotte", "Ella", "Emily", "Abigail", "Aria", "Aurora", "Brooklyn" };

        // Toddler 1 (13-24m): F-J
        var namesFJ_Male = new[] { "Frank", "George", "Henry", "Isaac", "Jack", "Finn", "Gabriel", "Gavin", "James", "Jacob", "Julian", "Jackson", "Jayden", "Joseph", "Joshua" };
        var namesFJ_Female = new[] { "Fiona", "Grace", "Hannah", "Ivy", "Julia", "Faith", "Hazel", "Isabella", "Harper", "Gianna", "Gabriella", "Hailey", "Isla", "Jasmine", "Jocelyn" };

        // Toddler 2 (25-36m): K-O
        var namesKO_Male = new[] { "Kevin", "Liam", "Mike", "Noah", "Oliver", "Kyle", "Leo", "Lucas", "Owen", "Logan", "Mason", "Matthew", "Nathan", "Nicholas", "Nolan" };
        var namesKO_Female = new[] { "Kate", "Lily", "Mia", "Nora", "Olivia", "Kayla", "Maya", "Natalie", "Naomi", "Nicole", "Madelyn", "Maria", "Melanie", "Mackenzie", "Katherine" };

        // Pre-K 1 (37-48m): P-T
        var namesPT_Male = new[] { "Paul", "Quinn", "Ryan", "Sam", "Tom", "Peter", "Parker", "Robert", "Tyler", "Patrick", "Preston", "Richard", "Samuel", "Sebastian", "Thomas" };
        var namesPT_Female = new[] { "Penelope", "Quincy", "Riley", "Sophia", "Thea", "Rose", "Peyton", "Piper", "Quinn", "Ruby", "Reagan", "Sarah", "Scarlett", "Stella", "Taylor" };

        // Pre-K 2 (49-60m): U-Z
        var namesUZ_Male = new[] { "Uriel", "Victor", "Will", "Xander", "Yusuf", "Zach", "Uriah", "Vincent", "Wyatt", "William", "Wesley", "Xavier", "Zane", "Zion", "Zachary" };
        var namesUZ_Female = new[] { "Ursula", "Violet", "Willow", "Xyla", "Yara", "Zoe", "Vanessa", "Victoria", "Vivian", "Valentina", "Willa", "Winter", "Zara", "Zaria", "Zuri" };

        int ageMonths = random.Next(minAgeMonths, maxAgeMonths + 1);
        var dob = DateTime.Now.AddMonths(-ageMonths);
        
        var classroom = classrooms.FirstOrDefault(c => ageMonths >= c.MinAgeMonths && ageMonths <= c.MaxAgeMonths);

        bool isMale = random.Next(2) == 0;
        string gender = isMale ? "Male" : "Female";
        string firstName = "Test";

        if (minAgeMonths <= 11) 
            firstName = isMale ? namesAE_Male[random.Next(namesAE_Male.Length)] : namesAE_Female[random.Next(namesAE_Female.Length)];
        else if (minAgeMonths <= 24) 
            firstName = isMale ? namesFJ_Male[random.Next(namesFJ_Male.Length)] : namesFJ_Female[random.Next(namesFJ_Female.Length)];
        else if (minAgeMonths <= 36) 
            firstName = isMale ? namesKO_Male[random.Next(namesKO_Male.Length)] : namesKO_Female[random.Next(namesKO_Female.Length)];
        else if (minAgeMonths <= 47) 
            firstName = isMale ? namesPT_Male[random.Next(namesPT_Male.Length)] : namesPT_Female[random.Next(namesPT_Female.Length)];
        else 
            firstName = isMale ? namesUZ_Male[random.Next(namesUZ_Male.Length)] : namesUZ_Female[random.Next(namesUZ_Female.Length)];

        return new Child
        {
            FirstName = firstName,
            LastName = lastName, // Match family name
            Gender = gender,
            DateOfBirth = dob,
            Parents = parents, // Assign the couple
            ClassroomId = classroom?.Id
        };
    }


}

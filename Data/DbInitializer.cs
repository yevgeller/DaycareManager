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
            new Classroom { ClassroomNumber = "Infant 1", MinAgeMonths = 0, MaxAgeMonths = 12 },
            new Classroom { ClassroomNumber = "Toddler 1", MinAgeMonths = 13, MaxAgeMonths = 24 },
            new Classroom { ClassroomNumber = "Toddler 2", MinAgeMonths = 25, MaxAgeMonths = 36 },
            new Classroom { ClassroomNumber = "Pre-K 1", MinAgeMonths = 37, MaxAgeMonths = 48 },
            new Classroom { ClassroomNumber = "Pre-K 2", MinAgeMonths = 49, MaxAgeMonths = 60 }
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
        var namesAE_First = new[] { "Alice", "Aaron", "Bella", "Benjamin", "Charlie", "Chloe", "David", "Daisy", "Ethan", "Emma", "Austin", "Adam", "Eva", "Blake", "Caleb" };

        // Toddler 1 (13-24m): F-J
        var namesFJ_First = new[] { "Frank", "Fiona", "George", "Grace", "Henry", "Hannah", "Isaac", "Ivy", "Jack", "Julia", "Finn", "Faith", "Gabriel", "Gavin", "Hazel" };

        // Toddler 2 (25-36m): K-O
        var namesKO_First = new[] { "Kevin", "Kate", "Liam", "Lily", "Mike", "Mia", "Noah", "Nora", "Oliver", "Olivia", "Kyle", "Kayla", "Leo", "Lucas", "Owen" };

        // Pre-K 1 (37-48m): P-T
        var namesPT_First = new[] { "Paul", "Penelope", "Quinn", "Quincy", "Ryan", "Riley", "Sam", "Sophia", "Tom", "Thea", "Peter", "Parker", "Rose", "Robert", "Tyler" };

        // Pre-K 2 (49-60m): U-Z
        var namesUZ_First = new[] { "Ursula", "Uriel", "Victor", "Violet", "Will", "Willow", "Xander", "Xyla", "Yusuf", "Yara", "Zach", "Zoe", "Uriah", "Vincent", "Wyatt" };

        int ageMonths = random.Next(minAgeMonths, maxAgeMonths + 1);
        var dob = DateTime.Now.AddMonths(-ageMonths);
        
        var classroom = classrooms.FirstOrDefault(c => ageMonths >= c.MinAgeMonths && ageMonths <= c.MaxAgeMonths);

        string firstName = "Test";

        if (minAgeMonths <= 11) firstName = namesAE_First[random.Next(namesAE_First.Length)];
        else if (minAgeMonths <= 24) firstName = namesFJ_First[random.Next(namesFJ_First.Length)];
        else if (minAgeMonths <= 36) firstName = namesKO_First[random.Next(namesKO_First.Length)];
        else if (minAgeMonths <= 47) firstName = namesPT_First[random.Next(namesPT_First.Length)];
        else firstName = namesUZ_First[random.Next(namesUZ_First.Length)];

        return new Child
        {
            FirstName = firstName,
            LastName = lastName, // Match family name
            DateOfBirth = dob,
            Parents = parents, // Assign the couple
            ClassroomId = classroom?.Id
        };
    }


}

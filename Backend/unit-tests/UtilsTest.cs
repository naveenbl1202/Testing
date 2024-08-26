using Xunit;
using System.Collections.Generic;  // For List<T>
using System.IO;                   // For File.ReadAllText
using Newtonsoft.Json;             // For JSON parsing (if using Newtonsoft.Json)
using System.Linq;                // For LINQ operations

namespace WebApp
{
    public class UtilsTest
    {
        // Read all mock users from file
        private static readonly List<MockUser> mockUsers = JsonConvert.DeserializeObject<List<MockUser>>(
            File.ReadAllText(Path.Combine("json", "mock-users.json"))
        );

        [Theory]
        [InlineData("abC9#fgh", true)]  // ok
        [InlineData("stU5/xyz", true)]  // ok too
        [InlineData("abC9#fg", false)]  // too short
        [InlineData("abCd#fgh", false)] // no digit
        [InlineData("abc9#fgh", false)] // no capital letter
        [InlineData("abC9efgh", false)] // no special character
        public void TestIsPasswordGoodEnough(string password, bool expected)
        {
            Assert.Equal(expected, Utils.IsPasswordGoodEnough(password));
        }

        [Theory]
        [InlineData("abC9#fgh", true)]  // ok
        [InlineData("stU5/xyz", true)]  // ok too
        [InlineData("abC9#fg", false)]  // too short
        [InlineData("abCd#fgh", false)] // no digit
        [InlineData("abc9#fgh", false)] // no capital letter
        [InlineData("abC9efgh", false)] // no special character
        public void TestIsPasswordGoodEnoughRegexVersion(string password, bool expected)
        {
            Assert.Equal(expected, Utils.IsPasswordGoodEnoughRegexVersion(password));
        }

        [Theory]
        [InlineData(
            "---",
            "Hello, I am going through hell. Hell is a real fucking place outside your goddamn comfy tortoiseshell!",
            "Hello, I am going through ---. --- is a real --- place outside your --- comfy tortoiseshell!"
        )]
        [InlineData(
            "---",
            "Rhinos have a horny knob? (or what should I call it) on their heads. And doorknobs are damn round.",
            "Rhinos have a --- ---? (or what should I call it) on their heads. And doorknobs are --- round."
        )]
        public void TestRemoveBadWords(string replaceWith, string original, string expected)
        {
            Assert.Equal(expected, Utils.RemoveBadWords(original, replaceWith));
        }

        [Fact]
        public void TestCreateMockUsers()
        {
            // Get all users from the database
            var usersInDb = MockDatabase.Query("SELECT email FROM users");
            var emailsInDb = usersInDb.Select(user => user.email).ToList();
            // Only keep the mock users not already in db
            var mockUsersNotInDb = mockUsers.Where(mockUser => !emailsInDb.Contains(mockUser.email)).ToList();
            // Get the result of running the method in our code
            var result = Utils.CreateMockUsers();
            // Assert that the CreateMockUsers only return newly created users in the db
            Console.WriteLine($"The test expected that {mockUsersNotInDb.Count} users should be added.");
            Console.WriteLine($"And {result.Count} users were added.");
            Console.WriteLine("The test also asserts that the users added are equivalent (the same) as the expected users!");
            Assert.True(mockUsersNotInDb.SequenceEqual(result), "The created mock users are not as expected.");
            Console.WriteLine("The test passed!");
        }

        [Fact]
        public void TestRemoveMockUsers()
        {
            // Get all users from the database before removing mock users
            var usersInDbBeforeRemoval = MockDatabase.Query("SELECT email FROM users");
            var emailsInDbBeforeRemoval = usersInDbBeforeRemoval.Select(user => user.email).ToList();

            // Filter out the mock users that are actually in the database
            var mockUsersInDb = mockUsers.Where(mockUser => emailsInDbBeforeRemoval.Contains(mockUser.email)).ToList();

            // Run the method to remove mock users
            var result = Utils.RemoveMockUsers();

            // Check that the number of removed mock users is correct
            Console.WriteLine($"The test expected that {mockUsersInDb.Count} users should be removed.");
            Console.WriteLine($"And {result.Count} users were removed.");
            Assert.True(mockUsersInDb.SequenceEqual(result), "The removed mock users are not as expected.");
            Console.WriteLine("The test passed!");
        }
    }

    // Dummy class to represent the structure of mock users
    public class MockUser : IEquatable<MockUser>
    {
        public string email { get; set; }

        public bool Equals(MockUser other)
        {
            if (other == null) return false;
            return email == other.email;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MockUser);
        }

        public override int GetHashCode()
        {
            return email?.GetHashCode() ?? 0;
        }
    }

    // Dummy methods for SQLQuery, Utils.IsPasswordGoodEnough, etc.
    public static class MockDatabase
    {
        public static List<MockUser> Query(string query)
        {
            // Placeholder for database query
            return new List<MockUser>();
        }
    }

    public static class Utils
    {
        public static bool IsPasswordGoodEnough(string password)
        {
            // Placeholder for password validation
            return true;
        }

        public static bool IsPasswordGoodEnoughRegexVersion(string password)
        {
            // Placeholder for password validation with regex
            return true;
        }

        public static string RemoveBadWords(string original, string replaceWith)
        {
            // Placeholder for bad words removal
            return original;
        }

        public static List<MockUser> CreateMockUsers()
        {
            // Placeholder for creating mock users
            return new List<MockUser>();
        }

        public static List<MockUser> RemoveMockUsers()
        {
            // Placeholder for removing mock users
            return new List<MockUser>();
        }
    }
}

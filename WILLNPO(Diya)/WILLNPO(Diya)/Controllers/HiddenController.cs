using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WILLNPO_Diya_.Models;

namespace WILLNPO_Diya_.Controllers
{
    public class HiddenController : Controller
    {
        // The Index action returns the list of team members to the view
        public IActionResult Index()
        {
            var teamMembers = new List<TeamMember>
            {
                new TeamMember
                {
                    Id = 1,
                    Name = "Alec Cowan",
                    Role = "App Devloper",
                    About = "Passionate about creating cutting edge software that will have a real life impact. Enjoy learning, growing and pushing the boundaries of technology",
                    FunFact = "I love nature and being outdoors more than anything — kinda ironic for someone who spends so much time with tech, right? Oh, and I also ride dirt bikes!",
                    ProfilePictureUrl = "/imagesPFP/AlecPfp.jpg",
                    CVUrl = "/cvs/CV Alec Cowan.pdf",
                    LinkedInUrl = "https://www.linkedin.com/in/alec-cowan-165402251/" // LinkedIn URL
                },
                new TeamMember
                {
                    Id = 2,
                    Name = "Alisa Thool",
                    Role = "Project Manager / Web Developer",
                    About = "I’m an animal enthusiast, adrenaline junkie, and a natural-born jokester. Whether I’m bonding with furry friends or chasing the next thrill, I bring a spark of humor and energy to every moment. Life’s better when you’re having fun, and I’m always up for the adventure!",
                    FunFact = "I jumped from a plan from 8000 feet!",
                    ProfilePictureUrl = "/imagesPFP/DiyaPfp.jpg",
                    CVUrl = "/cvs/Alisa D Thool - Resume.pdf",
                    LinkedInUrl = "https://www.linkedin.com/in/alisadiyathool/" // LinkedIn URL
                },
                new TeamMember
                {
                    Id = 3,
                    Name = "Anele Siwela",
                    Role = "Documentation",
                    About = "Developer and i run my own hair business on the side.",
                    FunFact = "Really enjoy beauty.",
                    ProfilePictureUrl = "/imagesPFP/AnelePfp.jpg",
                    CVUrl = "/cvs/Anele Siwela CV.pdf",
                    LinkedInUrl = "https://www.linkedin.com/in/anele-siwela" // LinkedIn URL
                },
                new TeamMember
                {
                    Id = 4,
                    Name = "Wandile Soji",
                    Role = "UX Designer",
                    About = "Dedicated to solve today's problems using technology.",
                    FunFact = "I can make farting noises with my hands.",
                    ProfilePictureUrl = "/imagesPFP/SojiPfp.jpg",
                    CVUrl = "/cvs/Wandile Soji - Resume.pdf",
                    LinkedInUrl = "https://www.linkedin.com/in/wandile-soji-506443247/" // LinkedIn URL
                }
            };

            return View(teamMembers); // Pass the teamMembers list to the view
        }

        // The DownloadCV action serves the CV file for the selected team member
        public IActionResult DownloadCV(int memberId)
        {
            // List of team members to find the correct CV file path
            var teamMembers = new List<TeamMember>
            {
                new TeamMember
                {
                    Id = 1,
                    Name = "Alec Cowan",
                    Role = "App Devloper",
                    About = "Passionate about creating cutting edge software that will have a real life impact. Enjoy learning, growing and pushing the boundaries of technology",
                    FunFact = "I love nature and being outdoors more than anything — kinda ironic for someone who spends so much time with tech, right? Oh, and I also ride dirt bikes!",
                    ProfilePictureUrl = "/imagesPFP/AlecPfp.jpg",
                    CVUrl = "/cvs/CV Alec Cowan.pdf",
                    LinkedInUrl = "https://www.linkedin.com/in/alec-cowan-165402251/" // LinkedIn URL
                },
                new TeamMember
                {
                    Id = 2,
                    Name = "Alisa Thool",
                    Role = "Project Manager / Web Developer",
                    About = "I’m an animal enthusiast, adrenaline junkie, and a natural-born jokester. Whether I’m bonding with furry friends or chasing the next thrill, I bring a spark of humor and energy to every moment. Life’s better when you’re having fun, and I’m always up for the adventure!",
                    FunFact = "I jumped from a plan from 8000 feet!",
                    ProfilePictureUrl = "/imagesPFP/DiyaPfp.jpg",
                    CVUrl = "/cvs/Alisa D Thool - Resume.pdf",
                    LinkedInUrl = "https://www.linkedin.com/in/alisadiyathool/" // LinkedIn URL
                },
                new TeamMember
                {
                    Id = 3,
                    Name = "Anele Siwela",
                    Role = "Documentation",
                    About = "Developer and i run my own hair business on the side.",
                    FunFact = "Really enjoy beauty.",
                    ProfilePictureUrl = "/imagesPFP/AnelePfp.jpg",
                    CVUrl = "/cvs/Anele Siwela CV.pdf",
                    LinkedInUrl = "https://www.linkedin.com/in/anele-siwela" // LinkedIn URL
                },
                new TeamMember
                {
                    Id = 4,
                    Name = "Wandile Soji",
                    Role = "UX Designer",
                    About = "Dedicated to solve today's problems using technology.",
                    FunFact = "I can make farting noises with my hands.",
                    ProfilePictureUrl = "/imagesPFP/SojiPfp.jpg",
                    CVUrl = "/cvs/Wandile Soji - Resume.pdf",
                    LinkedInUrl = "https://www.linkedin.com/in/wandile-soji-506443247/" // LinkedIn URL
                }
            };

            // Find the team member with the given Id
            var teamMember = teamMembers.FirstOrDefault(m => m.Id == memberId);
            if (teamMember == null)
            {
                return NotFound("Team member not found.");
            }

            // Build the full path to the CV file from the URL
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", teamMember.CVUrl.TrimStart('/'));

            // Check if the CV file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("CV not found.");
            }

            // Serve the CV file for download
            return PhysicalFile(filePath, "application/pdf", $"{teamMember.Name.Replace(" ", "_")}_CV.pdf");
        }
    }
}

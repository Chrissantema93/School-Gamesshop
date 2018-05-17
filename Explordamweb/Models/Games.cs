using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Explordamweb.Models
{
    public class Games
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter a name")]
        public string QueryName { get; set; }
        public string ReleaseDate { get; set; }
        public int RequiredAge { get; set; }
        public int DLCCount { get; set; }
        public int Metacritic { get; set; }
        public int RecommendationCount { get; set; }
        public string Platform { get; set; }
        public string Category { get; set; }
        [Required(ErrorMessage = "Please enter a Genre")]
        public string Genre { get; set; }
        public bool ControllerSupport { get; set; }
        public bool IsFree { get; set; }
        public bool PlatformWindows { get; set; }
        public bool PlatformLinux { get; set; }
        public bool PlatformMac { get; set; }
        public bool CategorySinglePlayer { get; set; }
        public bool CategoryMultiplayer { get; set; }
        public bool CategoryCoop { get; set; }
        public bool CategoryMMO { get; set; }
        public bool CategoryVRSupport { get; set; }
        public bool GenreIsNonGame { get; set; }
        public bool GenreIsIndie { get; set; }
        public bool GenreIsAction { get; set; }
        public bool GenreIsAdventure { get; set; }
        public bool GenreIsCasual { get; set; }
        public bool GenreIsStrategy { get; set; }
        public bool GenreIsRPG { get; set; }
        public bool GenreIsSimulation { get; set; }
        public bool GenreIsSports { get; set; }
        public bool GenreIsRacing { get; set; }
        public bool GenreIsMassivelyMultiplayer { get; set; }
        public string PriceCurrency { get; set; }
        public decimal PriceInitial { get; set; }
        [Required(ErrorMessage = "Please enter a Price")]
        public decimal PriceFinal { get; set; }
        public string SupportEmail { get; set; }
        public string SupportURL { get; set; }
        public string AboutText { get; set; }
        public string Background { get; set; }
        public string ShortDescrip { get; set; }
        public string DetailedDescrip { get; set; }
        public string HeaderImage { get; set; }
        public string LegalNotice { get; set; }
        public string Reviews { get; set; }
        public string SupportedLanguages { get; set; }
        public string Website { get; set; }
        public string PCMinReqsText { get; set; }
        public string PCRecReqsText { get; set; }
        public string LinuxMinReqsText { get; set; }
        public string LinuxRecReqsText { get; set; }
        public string MacMinReqsText { get; set; }
        public string MacRecReqsText { get; set; }

    }
}

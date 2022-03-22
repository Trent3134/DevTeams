using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class Program_UI
{
    private readonly DevTeamRepo _tRepo = new DevTeamRepo();
    private readonly DeveloperRepo _DevRepo = new DeveloperRepo();


    public void Run()
    {
        SeedData();

        runApplication();
    }

    private void runApplication()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            System.Console.WriteLine("---- welcome to Komodo-------");
            System.Console.WriteLine("please make a selection: \n" +
            "1. Add Developer to Database\n" +
            "2. View all Developers\n " +
            "3. View Developers by ID\n" +
            "4. Update Developers Data\n" +
            "5. Delete Developer Data\n" +
            "------------------------------\n" +
            "6. Add DevTeam to Database\n" +
            "7. View ALL DevTeams\n" +
            "8. View DevTeam by ID\n" +
            "9. Update DevTeam Data\n" +
            "10.Delete DevTeam Data\n" +
            "-------------------------\n" +
            "50. Close the Application");
        

        var userInput = Console.ReadLine();
        switch (userInput)
        {
            case "1":
                AddDeveloperToDatabase();
                break;
            case "2":
                ViewAllDevelopers();
                break;
            case "3":
                ViewAllDevelopersByID();
                break;
            case "4":
                UpdateDeveloperData();
                break;
            case "5":
                DeleteDeveloperData();
                break;
            case "6":
                AddDevTeamToDatabase();
                break;
            case "7":
                ViewAllDevTeams();
                break;
            case "8":
                ViewAllDevTeamsByID();
                break;
            case "9":
                UpdateDevTeamData();
                break;
            case "10":
                DeleteDevTeamData();
                break;
            case "50":
                isRunning = CloseApplication();
                break;
            default:
                System.Console.WriteLine("invaild Selection");
                PressAnyKeyToContinue();
                break;
        }
        }
    }

    private void DeleteDevTeamData()
    {
         Console.Clear();
        System.Console.WriteLine("--- Dev Team REMOVAL Page ---");

        var devTeams = _tRepo.GetAllDevTeams();
        foreach (DevTeam devTeam in devTeams)
        {
            DisplayDevTeamListing(devTeam);
        }
        try
        {
            System.Console.WriteLine("Please Select a dev Team by it's ID");
            var userInputSelectedDevTeam = int.Parse(Console.ReadLine());
            bool isSuccessful = _tRepo.RemoveDevTeamFromDatabase(userInputSelectedDevTeam);
            if (isSuccessful)
            {
                System.Console.WriteLine("DevTeam was succesffully deleted.");
            }
            else
            {
                System.Console.WriteLine("DevTeam Failed to be deleted. ");
            }
        }
        catch
        {
            System.Console.WriteLine("Sorry, invalid selection");
        }

        PressAnyKeyToContinue();
    }

    private void UpdateDevTeamData()
    {
        Console.Clear();

        var availDevTeam = _tRepo.GetAllDevTeams();
        foreach (var devTeam in availDevTeam)
        {
            DisplayDevTeamListing(devTeam);
        }
        System.Console.WriteLine("Please enter a valid devTeam ID: ");
        var userInputDevTeamID = int.Parse(Console.ReadLine());
        var userSelectedDevTeam = _tRepo.GetDevTeamByID(userInputDevTeamID);

        if (userSelectedDevTeam != null)
        {
            Console.Clear();
            var newDevTeam = new DevTeam();

            var currentDevelopers = _DevRepo.GetAllDevelopers();

            System.Console.WriteLine("Please enter a devTeam name");
            newDevTeam.TeamName = Console.ReadLine();

            bool hasAssignedDevelopers = false;
            while (!hasAssignedDevelopers)
            {
                System.Console.WriteLine("do you have any Developers? y/n");
                var userInputHasDevelopers = Console.ReadLine();

                if (userInputHasDevelopers == "Y".ToLower())
                {
                    foreach (var developer in currentDevelopers)
                    {
                        System.Console.WriteLine($"{developer.ID} {developer.FirstName} {developer.LastName} {developer.Pluralsight}");

                    }

                    var userInputDeveloperSelection = int.Parse(Console.ReadLine());
                    var selectedDeveloper = _DevRepo.GetDeveloperByID(userInputDeveloperSelection);

                    if (selectedDeveloper != null)
                    {
                        newDevTeam.Developers.Add(selectedDeveloper);
                        currentDevelopers.Remove(selectedDeveloper);
                    }
                    else
                    {
                        System.Console.WriteLine($"sorry, the developer with the ID: {userInputDeveloperSelection} does not exist");
                    }
                }
                else
                {
                    hasAssignedDevelopers = true;

                }

            }

            var isSuccessful = _tRepo.UpdateDevTeamData(userSelectedDevTeam.ID, newDevTeam);
            if (isSuccessful)
            {
                System.Console.WriteLine("SUCCESS!");
            }
            else
            {
                System.Console.WriteLine("FAIL");
            }
        }
        else
        {
            System.Console.WriteLine("FAIL");
        }

        PressAnyKeyToContinue();
    }

    private bool CloseApplication()
    {
        Console.Clear();
        System.Console.WriteLine("thank you!");
        PressAnyKeyToContinue();
        return false;
    }

    private void ViewAllDevTeamsByID()
    {
        Console.Clear();
        System.Console.WriteLine("--- DevTeam Details");

        var devTeams = _tRepo.GetAllDevTeams();
        foreach (DevTeam devTeam in devTeams)
        {
            DisplayDevTeamListing(devTeam);
        }
        try
        {
            System.Console.WriteLine("please select a devteam by it's ID");
            var userInputSelectedDevTeam = int.Parse(Console.ReadLine());
            var selectedDevTeam = _tRepo.GetDevTeamByID(userInputSelectedDevTeam);
            if (selectedDevTeam != null)
            {
                DisplayDevTeamListing(selectedDevTeam);
            }
            else
            {
                System.Console.WriteLine($"sorry the devteam with the ID: {userInputSelectedDevTeam} does not exist");
            }
        }
        catch
        {
            System.Console.WriteLine("sorry, invalid selection");

        }

        PressAnyKeyToContinue();
    }

    private void DisplayDevTeamListing(DevTeam devTeam)
    {
        System.Console.WriteLine($"DevTeamID: {devTeam.ID}\n DevTeam name: {devTeam.TeamName}\n" +
        "------------------------------------\n");
    }

    private void ViewAllDevTeams()
    {
        Console.Clear();
        System.Console.WriteLine("--- DevTeam Listings ---");
        var devTeamsInDb = _tRepo.GetAllDevTeams();

        foreach (var devTeam in devTeamsInDb)
        {
            DisplayDevTeamListing(devTeam);    
        }

        PressAnyKeyToContinue();
    }

    private void AddDevTeamToDatabase()
    {
        Console.Clear();
        var newdevTeam = new DevTeam();

        var currentDevelopers = _DevRepo.GetAllDevelopers();

        System.Console.WriteLine("Please enter a DevTeam Name");
        newdevTeam.TeamName = Console.ReadLine();

        bool hasAssignedDevelopers = false; 
        while(!hasAssignedDevelopers)
        {
            System.Console.WriteLine("do you have any Developers? y/n");
            var userInputHasDevelopers = Console.ReadLine();

            if (userInputHasDevelopers == "Y".ToLower())
            {
                foreach (var developer in currentDevelopers)
                {
                    System.Console.WriteLine($"{developer.ID} {developer.FirstName} {developer.LastName} {developer.Pluralsight}");
                }
                var userInputDeveloperSelection = int.Parse(Console.ReadLine());
                var selectedDeveloper = _DevRepo.GetDeveloperByID(userInputDeveloperSelection);

                if (selectedDeveloper != null)
                {
                    newdevTeam.Developers.Add(selectedDeveloper);
                    currentDevelopers.Remove(selectedDeveloper);
                }
                else
                {
                    System.Console.WriteLine($"Sorry, the developer with the ID: {userInputDeveloperSelection} does not exist");
                }
            }
            else
            {
                hasAssignedDevelopers = true; 
            }
        }

        bool isSuccessful = _tRepo.AddDevTeamToDatabase(newdevTeam);
        if (isSuccessful)
        {
            System.Console.WriteLine($"devTeam: {newdevTeam.Developers} was added to the database.");
        }
        else
        {
            System.Console.WriteLine("devteam failed to be added to the database");
        }
        PressAnyKeyToContinue();
    }

    private void DeleteDeveloperData()
    {
        Console.Clear();
        System.Console.WriteLine("--- Developer REMOVAL Page ---");

        var developers = _DevRepo.GetAllDevelopers();
        foreach (Developer developer in developers)
        {
            displayDeveloperInfo(developer);
        }
        try
        {
            System.Console.WriteLine("Please Select a Developer by it's ID");
            var userInputSelectedDeveloper = int.Parse(Console.ReadLine());
            bool isSuccessful = _DevRepo.RemoveDeveloperFromDatabase(userInputSelectedDeveloper);
            if (isSuccessful)
            {
                System.Console.WriteLine("Developer was succesffully deleted.");
            }
            else
            {
                System.Console.WriteLine("Developer Failed to be deleted. ");
            }
        }
        catch
        {
            System.Console.WriteLine("Sorry, invalid selection");
        }

        PressAnyKeyToContinue();
    }

    private void UpdateDeveloperData()
    {
        Console.Clear();

        var uvialDevelopers = _DevRepo.GetAllDevelopers();
        foreach (var developer in uvialDevelopers)
        {
            displayDeveloperInfo(developer);
        }

        System.Console.WriteLine("Please enter a valid developer ID: ");
        var userInputDeveloperID = int.Parse(Console.ReadLine());
        var userSelectedDeveloper = _DevRepo.GetDeveloperByID(userInputDeveloperID);

        if (userSelectedDeveloper != null)
        {
            Console.Clear();
            var newDeveloper = new Developer();

            var currentDevelopers = _DevRepo.GetAllDevelopers();

            System.Console.WriteLine("Please Enter A Developer name: ");

        }


    }

    private void ViewAllDevelopersByID()
    {
        Console.Clear();
        System.Console.WriteLine("--- Developer Detail Menu ---\n");
        System.Console.WriteLine("Please enter a Developer ID: \n");
        var userInputDeveloperID = int.Parse(Console.ReadLine());

        var developer = _DevRepo.GetDeveloperByID(userInputDeveloperID);

        if (developer != null)
        {
            displayDeveloperInfo(developer);

        }
        else
        {
            System.Console.WriteLine($"the Developer with the ID: {userInputDeveloperID} does not exist.");
        }

        PressAnyKeyToContinue();
    }

    private void displayDeveloperInfo(Developer developer)
    {
        System.Console.WriteLine($"DeveloperID: {developer.ID}\n" +
        $"DeveloperName: {developer.FirstName} {developer.LastName}\n  "+ 
        $"has pluralsight:  {developer.Pluralsight}\n" +
        "--------------------------------------------------\n");
    }
    private void ViewAllDevelopers()
    {
        Console.Clear();

        List<Developer> developersInDb = _DevRepo.GetAllDevelopers();

        if (developersInDb.Count > 0)
        {
            foreach (Developer developer in developersInDb)
            {
                displayDeveloperInfo(developer);
            }
        }
        else
        {
            System.Console.WriteLine("There are no developers");
        }

        PressAnyKeyToContinue();
    }


    private void AddDeveloperToDatabase()
    {
        Console.Clear();
        var newDeveloper = new Developer();
        System.Console.WriteLine("--- Developer Enlisting Form --- \n");

        System.Console.WriteLine("Please Enter a Developer First Name: ");
        newDeveloper.FirstName = Console.ReadLine();

        System.Console.WriteLine("Please Enter A Developer Last Name: ");
        newDeveloper.LastName = Console.ReadLine();

        System.Console.WriteLine(" has access to PluralSite");
        string userInputHasPluralSight = Console.ReadLine();
        if (userInputHasPluralSight == "Y".ToLower())
        {
            newDeveloper.Pluralsight = true;
        }
        else
        {
            newDeveloper.Pluralsight = false;
        }
        bool isSuccessful = _DevRepo.AddDeveloperToDataBase(newDeveloper);
        if (isSuccessful)
        {
            System.Console.WriteLine($"{newDeveloper.FirstName} - {newDeveloper.LastName} was added to the database!");
        }
        else
        {
            System.Console.WriteLine("Developer failed to be added to the database. ");
        }
        PressAnyKeyToContinue();
    }
    private void PressAnyKeyToContinue()
    {
        System.Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }

    private void SeedData()
    {
        var dwight = new Developer("dwight", "schrute",  true);
        var jim = new Developer("jim", "halpert", true );
        var michael = new Developer("michael", "scott", false);
        var pam = new Developer("pam", "halpert", true );
        var stanly = new Developer("stanly", "hundson", false);
        var creed = new Developer("creed", "braton", true);
        
        _DevRepo.AddDeveloperToDataBase(dwight);
        _DevRepo.AddDeveloperToDataBase(jim);
        _DevRepo.AddDeveloperToDataBase(michael);
        _DevRepo.AddDeveloperToDataBase(pam);
        _DevRepo.AddDeveloperToDataBase(stanly);
        _DevRepo.AddDeveloperToDataBase(creed);

        var teamBlue = new DevTeam("blue team",
        new List<Developer>
        {
            dwight,
            michael,
            creed
        });

        var teamGreen = new DevTeam("green team",
        new List<Developer>
        {
            jim,
            pam,
            stanly
        });
        

        _tRepo.AddDevTeamToDatabase(teamBlue);
        _tRepo.AddDevTeamToDatabase(teamGreen);

    }
    
}

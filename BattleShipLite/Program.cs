using ShipLibary.Models;
using ShipLibary;

namespace BattleShipLite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WelcomeMessage();
            PlayerInfoModel activePlayer=CreatePlayer("Player 1");
            PlayerInfoModel opponent=CreatePlayer("Player 2");
            PlayerInfoModel Winner=null;
            do{
              
                DisplayShotGrid(activePlayer);
              
                RecordPlayerShot(activePlayer,opponent);
                
              
                bool doesGameContinue=GameLogic.PlayerStillActive(opponent);
             
                if(doesGameContinue==true){
                    //swap positions
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    Winner=activePlayer;
                }
            }while(Winner==null);

            IdentifyWinner(Winner);
         }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UserName} for winning!");
            Console.WriteLine($"{winner.UserName} took {GameLogic.GetShotCount(winner)} shots");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot=false;
            string row="";
            int column=0;
            
            do{
               string shot=AskForShot();
               (row,column)=GameLogic.SplitShotIntoRowAndColumn(shot);  
               isValidShot=GameLogic.ValidateShot(activePlayer,row,column);

                if(isValidShot==false){
                     Console.WriteLine("Invalid shot location, please try again");
                } 
            }while(!isValidShot);
           
           bool isAHit=GameLogic.IdentifyShotResult(opponent, row, column);
           GameLogic.MarkShotResult(activePlayer,opponent,row,column,isAHit);
        }

        private static string AskForShot()
        {
            Console.Write("Enter your shot selection: ");
            string output=Console.ReadLine();   
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel avtivePlayer)
        {
            foreach(var gridSpot in avtivePlayer.ShotGrid){
                string currentRow=avtivePlayer.ShotGrid[0].SpotLetter;
               
                if(gridSpot.SpotLetter!=currentRow){
                    Console.WriteLine();
                    currentRow=gridSpot.SpotLetter;
                    }  
                     if(gridSpot.Status==GridSpotStatus.Empty){    
                        Console.Write($"{gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                 }
                 else if (gridSpot.Status==GridSpotStatus.Hit){
                     Console.Write("X");
                 }
                    else if(gridSpot.Status==GridSpotStatus.Miss){
                        Console.Write("O"); 
                }
                else {
                    Console.Write("?");
                }
            }
        }

        private static void WelcomeMessage(){
            Console.WriteLine("Welcome to Battle Ship Lite! ");
            Console.WriteLine("Created by Arafat Reshat");
            Console.WriteLine();

        }
        private static PlayerInfoModel CreatePlayer(string playerTitle){
            PlayerInfoModel output=new PlayerInfoModel();

            Console.WriteLine($"Player infotmation for {playerTitle} ");
            //ask the user for their name
            output.UserName=AskForUsersName();
            //load up the shot grid
            GameLogic.IntializeGrid(output);
            //output.ShotGrid=
            //ask the user for their 5 ship placements
            PlaceShips(output);

            Console.Clear();

            return output;

        } 

        private static string AskForUsersName(){
            Console.Write("Enter your name: ");
            string output=Console.ReadLine();
            return output;
        }

        private static void PlaceShips(PlayerInfoModel model){
           do{
                Console.Write("Where do you want to place your ship number {model.ShipLocations.Count+1} : ");
                string location=Console.ReadLine();
                bool isValidLocation=GameLogic.PlaceShip(model,location);

                if(isValidLocation==false){
                    Console.WriteLine("That is not a valid location, type again");
                }


           }while(model.ShipLocations.Count<5);
        }
    }
}
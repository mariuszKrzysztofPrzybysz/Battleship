namespace BattleShip.Repository.RepositoryHelpers
{
    public class Result
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public dynamic Data { get; set; }
    }
}
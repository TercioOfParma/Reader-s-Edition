public class Result
{
    public bool Ok {get; set;}
    public string Message {get; set;}

    private Result()
    {
        
    }
    public static Result Success()
    {
        var result = new Result();
        result.Ok = true;
        result.Message = "Success";

        return result;
    }
    public static Result Error(string message)
    {
        var result = new Result();
        result.Ok = false;
        result.Message = message;

        return result;
    }
}
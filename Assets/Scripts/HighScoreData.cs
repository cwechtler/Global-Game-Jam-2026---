// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class HighScoreData
{
	public int Id { get; set; }
	public string Name { get; set; }
	public int Score { get; set; }
}
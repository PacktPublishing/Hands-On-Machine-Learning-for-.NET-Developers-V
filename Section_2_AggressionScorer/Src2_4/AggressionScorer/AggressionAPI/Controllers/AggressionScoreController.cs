using Microsoft.AspNetCore.Mvc;

namespace AggressionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AggressionScoreController : ControllerBase
    {
        public AggressionScoreController()
        {

        }

        [HttpPost]
        public JsonResult Post([FromBody] CommentWrapper input)
        {
            return new JsonResult(ScoreComment(input.Comment));
        }

        private AggressionPrediction ScoreComment(string comment)
        {
            return new AggressionPrediction()
            {
                IsAggressive = false,
                Probability = -1
            };
        }
    }

    public class AggressionPrediction
    {
        public bool IsAggressive { get; set; }
        public float Probability { get; set; }
    }

    public class CommentWrapper
    {
        public string Comment { get; set; }
    }
}

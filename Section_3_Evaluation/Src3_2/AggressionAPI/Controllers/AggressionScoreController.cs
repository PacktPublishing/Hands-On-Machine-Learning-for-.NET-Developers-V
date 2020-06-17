using AggressionScorerModel;
using Microsoft.AspNetCore.Mvc;

namespace AggressionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AggressionScoreController : ControllerBase
    {
        private readonly AggressionScorer _aggressionScorer;

        public AggressionScoreController(AggressionScorer aggressionScorer)
        {
            _aggressionScorer = aggressionScorer;
        }

        [HttpPost]
        public JsonResult Post([FromBody] CommentWrapper input)
        {
            return new JsonResult(ScoreComment(input.Comment));
        }

        private AggressionPrediction ScoreComment(string comment)
        {

            var classification = _aggressionScorer.Predict(comment);

            return new AggressionPrediction()
            {
                IsAggressive = classification.Prediction,
                Probability = classification.Probability
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

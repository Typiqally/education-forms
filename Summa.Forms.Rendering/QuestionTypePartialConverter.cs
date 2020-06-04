using System;
using Summa.Forms.Models;

namespace Summa.Forms.Rendering
{
    public class QuestionTypePartialConverter : IEnumTypeConverter<QuestionType>
    {
        public string ConvertToString(QuestionType type)
        {
            return type switch
            {
                QuestionType.MultipleChoice => "_MultipleChoiceQuestionPartial",
                QuestionType.LinearScale => "_LinearScaleQuestionPartial",
                QuestionType.Open => "_OpenQuestionPartial",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public QuestionType FromString(string str)
        {
            throw new NotImplementedException();
        }
    }
}
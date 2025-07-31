using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObject.Service
{
    public class SurveyService
    {
        private readonly DrugUsePreventionSupportSystemContext _context;

        public SurveyService()
        {
            _context = new DrugUsePreventionSupportSystemContext();
        }

        public SurveyService(DrugUsePreventionSupportSystemContext context)
        {
            _context = context;
        }

        // Survey Operations
        public async Task<List<Survey>> GetAllSurveysAsync()
        {
            try
            {
                return await _context.Surveys
                    .Include(s => s.SurveyQuestions)
                    .OrderBy(s => s.SurveyName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải danh sách khảo sát: {ex.Message}", ex);
            }
        }

        public async Task<Survey?> GetSurveyByIdAsync(int surveyId)
        {
            try
            {
                return await _context.Surveys
                    .Include(s => s.SurveyQuestions)
                        .ThenInclude(q => q.SurveyAnswers)
                    .FirstOrDefaultAsync(s => s.SurveyId == surveyId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải khảo sát: {ex.Message}", ex);
            }
        }

        public async Task<Survey> AddSurveyAsync(Survey survey)
        {
            try
            {

                _context.Surveys.Add(survey);
                await _context.SaveChangesAsync();
                return survey;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm khảo sát: {ex.Message}", ex);
            }
        }

        public async Task UpdateSurveyAsync(Survey survey)
        {
            try
            {
                _context.Entry(survey).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật khảo sát: {ex.Message}", ex);
            }
        }

        public async Task DeleteSurveyAsync(int surveyId)
        {
            try
            {
                var survey = await _context.Surveys
                    .Include(s => s.SurveyQuestions)
                        .ThenInclude(q => q.SurveyAnswers)
                    .Include(s => s.UserSurveyResults)
                    .FirstOrDefaultAsync(s => s.SurveyId == surveyId);

                if (survey != null)
                {
                    // EF Core will handle cascade delete for related entities
                    _context.Surveys.Remove(survey);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa khảo sát: {ex.Message}", ex);
            }
        }

        public async Task<int> GetQuestionCountAsync(int surveyId)
        {
            try
            {
                return await _context.SurveyQuestions
                    .CountAsync(q => q.SurveyId == surveyId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đếm câu hỏi: {ex.Message}", ex);
            }
        }

        // Question Operations
        public async Task<List<SurveyQuestion>> GetQuestionsBySurveyAsync(int surveyId)
        {
            try
            {
                return await _context.SurveyQuestions
                    .Include(q => q.SurveyAnswers)
                    .Where(q => q.SurveyId == surveyId)
                    .OrderBy(q => q.QuestionId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải câu hỏi: {ex.Message}", ex);
            }
        }

        public async Task<SurveyQuestion?> GetQuestionByIdAsync(int questionId)
        {
            try
            {
                return await _context.SurveyQuestions
                    .Include(q => q.SurveyAnswers)
                    .FirstOrDefaultAsync(q => q.QuestionId == questionId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải câu hỏi: {ex.Message}", ex);
            }
        }

        public async Task<SurveyQuestion> AddQuestionAsync(SurveyQuestion question)
        {
            try
            {
                _context.SurveyQuestions.Add(question);
                await _context.SaveChangesAsync();

                // Load the question with its survey for navigation properties
                await _context.Entry(question)
                    .Reference(q => q.Survey)
                    .LoadAsync();

                return question;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm câu hỏi: {ex.Message}", ex);
            }
        }

        public async Task UpdateQuestionAsync(SurveyQuestion question)
        {
            try
            {
                _context.Entry(question).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật câu hỏi: {ex.Message}", ex);
            }
        }

        public async Task DeleteQuestionAsync(int questionId)
        {
            try
            {
                var question = await _context.SurveyQuestions
                    .Include(q => q.SurveyAnswers)
                    .FirstOrDefaultAsync(q => q.QuestionId == questionId);

                if (question != null)
                {
                    // EF Core will handle cascade delete for related answers
                    _context.SurveyQuestions.Remove(question);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa câu hỏi: {ex.Message}", ex);
            }
        }

        // Answer Operations
        public async Task<List<SurveyAnswer>> GetAnswersByQuestionAsync(int questionId)
        {
            try
            {
                return await _context.SurveyAnswers
                    .Where(a => a.QuestionId == questionId)
                    .OrderBy(a => a.AnswerId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải đáp án: {ex.Message}", ex);
            }
        }

        public async Task<SurveyAnswer?> GetAnswerByIdAsync(int answerId)
        {
            try
            {
                return await _context.SurveyAnswers
                    .FirstOrDefaultAsync(a => a.AnswerId == answerId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tải đáp án: {ex.Message}", ex);
            }
        }

        public async Task<SurveyAnswer> AddAnswerAsync(SurveyAnswer answer)
        {
            try
            {
                _context.SurveyAnswers.Add(answer);
                await _context.SaveChangesAsync();

                // Load the answer with its question for navigation properties
                await _context.Entry(answer)
                    .Reference(a => a.Question)
                    .LoadAsync();

                return answer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm đáp án: {ex.Message}", ex);
            }
        }

        public async Task UpdateAnswerAsync(SurveyAnswer answer)
        {
            try
            {
                _context.Entry(answer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật đáp án: {ex.Message}", ex);
            }
        }

        public async Task DeleteAnswerAsync(int answerId)
        {
            try
            {
                var answer = await _context.SurveyAnswers
                    .FirstOrDefaultAsync(a => a.AnswerId == answerId);

                if (answer != null)
                {
                    _context.SurveyAnswers.Remove(answer);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa đáp án: {ex.Message}", ex);
            }
        }

        // Additional utility methods
        public async Task<List<Survey>> SearchSurveysAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllSurveysAsync();

                return await _context.Surveys
                    .Include(s => s.SurveyQuestions)
                    .Where(s => s.SurveyName.Contains(searchTerm) ||
                               s.Description.Contains(searchTerm))
                    .OrderBy(s => s.SurveyName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm kiếm khảo sát: {ex.Message}", ex);
            }
        }

        public async Task<bool> SurveyExistsAsync(int surveyId)
        {
            try
            {
                return await _context.Surveys.AnyAsync(s => s.SurveyId == surveyId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra khảo sát: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

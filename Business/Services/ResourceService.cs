using Business.Models;
using Business.Results;
using Business.Results.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Business.Services
{
    public interface IResourceService
    {
        IQueryable<ResourceModel> Query();
        Result Add(ResourceModel model);
        Result Update(ResourceModel model);
        Result Delete(int id);

        // extra method definitions can be added to use in the related controller and
        // to implement to the related classes
        List<ResourceModel> GetList();
    }

    public class ResourceService : IResourceService
    {
        private readonly Db _db;

        public ResourceService(Db db)
        {
            _db = db;
        }

        public IQueryable<ResourceModel> Query()
        {
            // TODO: Users count (UserCountOutput)
            return _db.Resources.Select(r => new ResourceModel()
            {
                Content = r.Content,
                Date = r.Date,
                Id = r.Id,
                Score = r.Score,
                Title = r.Title,

                //ScoreOutput = r.Score.ToString("N1", new CultureInfo("en-US"))
                // no need to use CultureInfo anywhere in our project anymore,
                // since we made the culture info configuration under the
                // Localization section in MVC project's Program.cs file
                ScoreOutput = r.Score.ToString("N1"), 
                // N: number format, 1: one decimal after decimal point

                DateOutput = r.Date.HasValue ? r.Date.Value.ToString("MM/dd/yyyy hh:mm:ss") : ""
                // MM: 2 digits month, dd: 2 digits day, yyyy: 4 digits year,
                // hh: 12 hour with AM and PM, mm: 2 digits minute, ss: 2 digits second
            }).OrderByDescending(r => r.Date).ThenByDescending(r => r.Score);
        }

        public Result Add(ResourceModel model)
        {
            // TODO: UserResources relation
            // we want to check whether a resource with the same title exists for the same date without time
            // therefore we use the Date property of DateTime type which is returned by the GetValueOrDefault method,
            // this method can be used with nullable DateTime type which returns the DateTime value
            // or the default value (01/01/0001 00:00:00.000) if the value is null 
            if (_db.Resources.Any(r => r.Date.GetValueOrDefault().Date == model.Date.GetValueOrDefault().Date &&
                r.Title.ToUpper() == model.Title.ToUpper().Trim()))
                return new ErrorResult("Resource with the same title and date exists!");

            var entity = new Resource()
            {
                // ? can be used if the value of a property can be null,
                // if model.Content is null, Content is set to null, else Content is set to
                // model.Content's trimmed value
                Content = model.Content?.Trim(),
                
                Date = model.Date,
                Score = model.Score,
                Title = model.Title.Trim() // since Title is required in the model, therefore can't be null,
                                           // we don't need to use ?
            };

            _db.Resources.Add(entity);
            _db.SaveChanges();

            return new SuccessResult("Resource added successfully.");
        }

        public Result Update(ResourceModel model)
        {
            // TODO: UserResources relation
            if (_db.Resources.Any(r => r.Date.GetValueOrDefault().Date == model.Date.GetValueOrDefault().Date &&
               r.Title.ToUpper() == model.Title.ToUpper().Trim() && r.Id != model.Id))
                return new ErrorResult("Resource with the same title and date exists!");
            var entity = new Resource()
            {
                Id = model.Id, // must be set for update
                Content = model.Content?.Trim(),
                Date = model.Date,
                Score = model.Score,
                Title = model.Title.Trim()
            };
            _db.Resources.Update(entity);
            _db.SaveChanges();
            return new SuccessResult("Resource updated successfully.");
        }

        public Result Delete(int id)
        {
            var entity = _db.Resources.Include(r => r.UserResources).SingleOrDefault(r => r.Id == id);
            if (entity is null)
                return new ErrorResult("Resource not found!");

            // deleting relational UserResource entities of the resource entity first
            _db.UserResources.RemoveRange(entity.UserResources);

            // then deleting the Resource entity
            _db.Resources.Remove(entity);

            _db.SaveChanges();

            return new SuccessResult("Resource deleted successfully.");
        }

        public List<ResourceModel> GetList()
        {
            // since we wrote the Query method above, we should call it
            // and return the result as a list by calling ToList method
            return Query().ToList();
        }
    }
}

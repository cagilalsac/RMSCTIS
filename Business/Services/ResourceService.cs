﻿using Business.Models;
using DataAccess.Results;
using DataAccess.Results.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Business.Services.Bases;

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
        ResourceModel GetItem(int id);
	}

    public class ResourceService : ServiceBase, IResourceService
    {
        // Way 1: injecting manually without ServiceBase inheritance
        //private readonly Db _db;

        //public ResourceService(Db db)
        //{
        //    _db = db;
        //}



        // Way 2: injecting with ServiceBase inheritance
        public ResourceService(Db db) : base(db)
        {
        }



        public IQueryable<ResourceModel> Query()
        {
            return _db.Resources.Include(r => r.UserResources).Select(r => new ResourceModel()
            {
                Content = r.Content,
                Date = r.Date,
                Id = r.Id,
                Score = r.Score,
                Title = r.Title,

                //ScoreOutput = r.Score.ToString("N1", new CultureInfo("en-US"))
                // CultureInfo should be used when formatting decimal and date time values to string.
                // No need to use CultureInfo anywhere in our project anymore, since we made the culture info
                // configuration in the MvcControllerBase class
                ScoreOutput = r.Score.ToString("N1"), 
                // N: number format, 1: one decimal after decimal point

                DateOutput = r.Date.HasValue ? r.Date.Value.ToString("MM/dd/yyyy hh:mm:ss") : "",
                // MM: 2 digits month, dd: 2 digits day, yyyy: 4 digits year,
                // hh: 12 hour with AM and PM, mm: 2 digits minute, ss: 2 digits second

                UserCountOutput = r.UserResources.Count,

                // querying over many to many relationship
                UserNamesOutput = string.Join("<br />", r.UserResources.Select(ur => ur.User.UserName)), // to show user names in details operation
                UserIdsInput = r.UserResources.Select(ur => ur.UserId).ToList() // to set selected UserIds in edit operation
            }).OrderByDescending(r => r.Date).ThenByDescending(r => r.Score);
        }

        public Result Add(ResourceModel model)
        {
            // we want to check whether a resource with the same title exists for the same date without time
            // therefore we use the Date property of DateTime type,
            // for entity's delegate of type Resource (r), we create a new DateTime with default value
            // "01/01/0001 00:00:00.000" if its value is null
            // and check the delegate's Date property value is equal to the model's Date property value
            if (model.Date.HasValue && 
                _db.Resources.Any(r => (r.Date ?? new DateTime()).Date == model.Date.Value.Date &&
                r.Title.ToUpper() == model.Title.ToUpper().Trim()))
                return new ErrorResult("Resource with the same title and date exists!");

            var entity = new Resource()
            {
                // ? can be used if the value of a property can be null,
                // if model.Content is null, Content is set to null, else Content is set to
                // model.Content's trimmed value
                Content = model.Content?.Trim(),
                
                Date = model.Date,
                Score = model.Score ?? 0,
                Title = model.Title.Trim(), // since Title is required in the model, therefore can't be null,
                                            // we don't need to use ?

                // inserting many to many relational entity,
				// ? must be used with UserIdsInput if there is a possibility that it can be null
                UserResources = model.UserIdsInput?.Select(userId => new UserResource()
                {
                    UserId = userId
                }).ToList()
            };

            _db.Resources.Add(entity);
            _db.SaveChanges();

            return new SuccessResult("Resource added successfully.");
        }

        public Result Update(ResourceModel model)
        {
            if (model.Date.HasValue &&
                _db.Resources.Any(r => (r.Date ?? new DateTime()).Date == model.Date.Value.Date &&
                r.Title.ToUpper() == model.Title.ToUpper().Trim() && r.Id != model.Id))
                return new ErrorResult("Resource with the same title and date exists!");

            // deleting many to many relational entity
            var existingEntity = _db.Resources.Include(r => r.UserResources).SingleOrDefault(r => r.Id == model.Id);
            if (existingEntity is not null && existingEntity.UserResources is not null)
                _db.UserResources.RemoveRange(existingEntity.UserResources);

			// existingEntity queried from the database must be updated since we got the existingEntity
			// first as above, therefore changes of the existing entity are being tracked by Entity Framework,
			// if disabling of change tracking is required, AsNoTracking method must be used after the DbSet,
			// for example _db.Resources.AsNoTracking()
            existingEntity.Content = model.Content?.Trim();
            existingEntity.Date = model.Date;
            existingEntity.Score = model.Score ?? 0;
            existingEntity.Title = model.Title.Trim();

            // inserting many to many relational entity
            existingEntity.UserResources = model.UserIdsInput?.Select(userId => new UserResource()
            {
                UserId = userId
            }).ToList();

            _db.Resources.Update(existingEntity);
            _db.SaveChanges(); // changes in all DbSets are commited to the database by Unit of Work

            return new SuccessResult("Resource updated successfully.");
        }

        public Result Delete(int id)
        {
            var entity = _db.Resources.Include(r => r.UserResources).SingleOrDefault(r => r.Id == id);
            if (entity is null)
                return new ErrorResult("Resource not found!");

            // deleting many to many relational entity:
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

        // Way 1:
        //public ResourceModel GetItem(int id)
        //{
        //    return Query().SingleOrDefault(r => r.Id == id);
        //}
        // Way 2:
        public ResourceModel GetItem(int id) => Query().SingleOrDefault(r => r.Id == id);
    }
}

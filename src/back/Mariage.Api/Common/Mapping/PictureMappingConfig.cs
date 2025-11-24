using Azure.Core;
using Mapster;
using Mariage.Application.Authentication.Common;
using Mariage.Application.Pictures.Commands.AddPicturesToFavorites;
using Mariage.Application.Pictures.Commands.CreatePicture;
using Mariage.Application.Pictures.Commands.RemovePictureFromFavorites;
using Mariage.Application.Pictures.Common;
using Mariage.Application.Pictures.Queries;
using Mariage.Application.Pictures.Queries.GetFavoritesPictures;
using Mariage.Application.Pictures.Queries.GetPicturesTookByUser;
using Mariage.Contracts.Authentication;
using Mariage.Contracts.Pictures;
using Mariage.Domain.PictureAggregate;
using Mariage.Domain.PictureAggregate.ValueObject;
using Mariage.Domain.UserAggregate;
using Mariage.Domain.UserAggregate.ValueObjects;

namespace Mariage.Api.Common.Mapping;

public class PictureMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(int Page, int PageSize, Guid User), GetPicturesTookByUserQuery>()
            .Map(dest => dest.Page, src => src.Page)
            .Map(dest => dest.UserId, src => UserId.Create(src.User))
            .Map(dest => dest.PageSize, src => src.PageSize);
        
        config.NewConfig<(GetPicturesPaginated Picture, Guid User), GetPictureQuery>()
            .Map(dest => dest.Page, src => src.Picture.Page)
            .Map(dest => dest.PageSize, src => src.Picture.PageSize);
        
        config.NewConfig<(int Page, int PageSize, Guid User), GetFavoritePicturesQuery>()
            .Map(dest => dest.Page, src => src.Page)
            .Map(dest => dest.UserId, src => UserId.Create(src.User))
            .Map(dest => dest.PageSize, src => src.PageSize);
        
        config.NewConfig<(string UrlPicture, UserId UserId), CreatePictureCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.UrlPicture, src => src.UrlPicture);

        config.NewConfig<(Picture Picture, User User), PictureResult>()
            .Map(dest => dest.Username, src => src.User.Username)
            .Map(dest => dest.IsFavorite, src => src.User.PictureIds.Contains(src.Picture.Id))
            .Map(dest => dest.Id, src => src.Picture.Id.Value)
            .Map(dest => dest.UrlImage, src => src.Picture.UrlImage);

        config.NewConfig<(Guid PictureId, Guid User), AddPicturesToFavoritesCommand>()
            .Map(dest => dest.UserId, src => UserId.Create(src.User))
            .Map(dest => dest.PictureId, src => PictureId.Create(src.PictureId));

        config.NewConfig<(Guid PictureId, Guid User), RemovePictureFromFavoritesCommand>()
            .Map(dest => dest.UserId, src => UserId.Create(src.User))
            .Map(dest => dest.PictureId, src => PictureId.Create(src.PictureId));

        config.NewConfig<string, PicturePhotoBoothResult>()
            .Map(dest => dest.UrlImage, src => src);
    }
}
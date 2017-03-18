﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GraphQL.Types;

namespace StarWars.Api.Models
{
    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType(Core.Data.ICharacterRepository characterRepository, IMapper mapper)
        {
            Name = "Droid";
            Description = "A mechanical creature in the Star Wars universe.";

            Field(x => x.Id).Description("The Id of the Droid.");
            Field(x => x.Name, nullable: true).Description("The name of the Droid.");

            Field<ListGraphType<CharacterInterface>>(
                "friends",
                resolve: context =>
                {
                    var friends = characterRepository.GetFriends(int.Parse(context.Source.Id)).Result;
                    var mapped = mapper.Map<IEnumerable<Character>>(friends);
                    return mapped;
                }
            );

            Field(x => x.AppearsIn).Resolve(
                context =>
                {
                    var episodes = characterRepository.GetEpisodes(int.Parse(context.Source.Id)).Result;
                    var titles = episodes.Select(y => y.Title).ToArray();
                    return titles;
                }
            ).Description("Which movie they appear in.");


            Field(d => d.PrimaryFunction, nullable: true).Description("The primary function of the droid.");

            Interface<CharacterInterface>();
        }
    }
}

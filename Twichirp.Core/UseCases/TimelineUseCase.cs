// Copyright (c) 2016-2017 meil
//
// This file is part of Twichirp.
// 
// Twichirp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Twichirp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Twichirp.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twichirp.Core.DataObjects;
using Twichirp.Core.Repositories;
using Twichirp.Core.Services;

namespace Twichirp.Core.UseCases {

    public class TimelineUseCase {

        private ITwitterEventService twitterEventService;
        private ITimelineRepository defaultRepository;

        public TimelineUseCase(ITwitterEventService twitterEventService,ITimelineRepository defaultRepository) {
            this.twitterEventService = twitterEventService;
            this.defaultRepository = defaultRepository;
        }

        public async Task<IEnumerable<CoreTweet.Status>> Load(ImmutableAccount account,int count,long? sinceId = null,long? maxId = null,ITimelineRepository repository=null) {
            repository = repository ?? defaultRepository;
            return await repository.Load(account,count,sinceId,maxId);
        }
    }
}

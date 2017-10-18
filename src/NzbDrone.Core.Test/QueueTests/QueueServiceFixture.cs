using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NzbDrone.Core.Download.TrackedDownloads;
using NzbDrone.Core.Queue;
using NzbDrone.Core.Test.Framework;
using FizzWare.NBuilder;
using FluentAssertions;
using NzbDrone.Core.Music;
using NzbDrone.Core.Parser.Model;

namespace NzbDrone.Core.Test.QueueTests
{
    [TestFixture]
    public class QueueServiceFixture : CoreTest<QueueService>
    {
        private List<TrackedDownload> _trackedDownloads;

        [SetUp]
        public void SetUp()
        {
            var downloadItem = Builder<NzbDrone.Core.Download.DownloadClientItem>.CreateNew()
                                        .With(v => v.RemainingTime = TimeSpan.FromSeconds(10))
                                        .Build();

            var series = Builder<Artist>.CreateNew()
                                        .Build();

            var episodes = Builder<Album>.CreateListOfSize(3)
                                          .All()
                                          .With(e => e.ArtistId = series.Id)
                                          .Build();
            
            var remoteEpisode = Builder<RemoteAlbum>.CreateNew()
                                                   .With(r => r.Artist = series)
                                                   .With(r => r.Albums = new List<Album>(episodes))
                                                   .With(r => r.ParsedAlbumInfo = new ParsedAlbumInfo())
                                                   .Build();

            _trackedDownloads = Builder<TrackedDownload>.CreateListOfSize(1)
                .All()
                .With(v => v.DownloadItem = downloadItem)
                .With(v => v.RemoteAlbum = remoteEpisode)
                .Build()
                .ToList();
        }

        [Test]
        public void queue_items_should_have_id()
        {
            Subject.Handle(new TrackedDownloadRefreshedEvent(_trackedDownloads));

            var queue = Subject.GetQueue();

            queue.Should().HaveCount(3);

            queue.All(v => v.Id > 0).Should().BeTrue();

            var distinct = queue.Select(v => v.Id).Distinct().ToArray();

            distinct.Should().HaveCount(3);
        }
    }
}

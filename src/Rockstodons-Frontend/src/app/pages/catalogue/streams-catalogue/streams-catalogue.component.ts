import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IStream } from 'src/app/core/interfaces/streams/stream';
import { IStreamDetailsDTO } from 'src/app/core/interfaces/streams/stream-details-dto';
import { ITrackDetailsDTO } from 'src/app/core/interfaces/tracks/track-details-dto';
import { StreamsService } from 'src/app/core/services/streams.service';

@Component({
  selector: 'app-streams-catalogue',
  templateUrl: './streams-catalogue.component.html',
  styleUrls: ['./streams-catalogue.component.scss'],
})
export class StreamsCatalogueComponent {
  streamsForCatalogue!: IStreamDetailsDTO[];

  isLoading = false;

  gridSelectionStyle = {
    width: '95%',
    textAlign: 'center',
  };

  fallback = '../../../assets/images/alternative-image.png';

  audioStreamingList!: ITrackDetailsDTO[];

  constructor(private streamsService: StreamsService) {

  }

  addCurrentAudioStreamingList(tracksForStreaming: ITrackDetailsDTO[]): void {
    this.audioStreamingList = [...tracksForStreaming];
  }

  ngOnInit(): void {
    this.retrieveStreamsCatalogueData();
  }

  private retrieveStreamsCatalogueData(): void {
    this.isLoading = true;
    this.streamsService.getStreamsWithFullDetails().subscribe((data) => {
      this.streamsForCatalogue = [];
      data
        .filter((stream: any) => !stream.isDeleted && stream.streamTracks.length > 0)
        .map((stream: any) => {
          const streamForCatalogue = {
            id: stream.id,
            name: stream.name,
            imageFileName: stream.imageFileName,
            imageUrl: stream.imageUrl,
            tracks: stream.streamTracks.map((streamTrack: any) => {
              const mappedTrackDetails = {
                url: streamTrack.track.audioFileUrl,
                title: streamTrack.track.name,
                cover: streamTrack.track.album.imageUrl,
              } as ITrackDetailsDTO;
              return mappedTrackDetails;
            }),
          } as IStreamDetailsDTO;
          this.streamsForCatalogue.push(streamForCatalogue);
        });
      this.audioStreamingList = this.streamsForCatalogue[0].tracks;
      this.isLoading = false;
    });
  }
}

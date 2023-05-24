import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IStream } from 'src/app/core/interfaces/streams/stream';
import { IStreamDetailsDTO } from 'src/app/core/interfaces/streams/stream-details-dto';
import { ITrackDetailsDTO } from 'src/app/core/interfaces/tracks/track-details-dto';
import { StreamsService } from 'src/app/core/services/streams.service';
//@ts-ignore
import * as amplitude from 'amplitudejs';
import { DOCUMENT } from '@angular/common';
import { AudioPlayerService } from 'src/app/core/services/audio-player.service';

@Component({
  selector: 'app-streams-catalogue',
  templateUrl: './streams-catalogue.component.html',
  styleUrls: ['./streams-catalogue.component.scss'],
})
export class StreamsCatalogueComponent {
  streamsForCatalogue!: IStreamDetailsDTO[];

  initialSong: any;
  currentTrack: any;
  volumeIcon = 'play-circle';
  showPlaylist = 'open-right-sidebar';
  playerClass = 'player-primary';

  isLoading = false;

  gridSelectionStyle = {
    width: '95%',
    textAlign: 'center',
  };

  fallback = '../../../assets/images/alternative-image.png';

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private streamsService: StreamsService,
    private audioPlayerService: AudioPlayerService
  ) {

  }

  changeVolumeIcon(event: any) {
    const value = event.target.value;
    if (value < 1) {
      this.volumeIcon = 'pause-circle';
    } else if (value > 0 && value < 70) {
      this.volumeIcon = 'down';
    } else if (value > 70) {
      this.volumeIcon = 'up';
    }
  }

  openPlaylist() {
    if (this.document.body.classList.contains(this.showPlaylist)) {
      this.document.body.classList.remove(this.showPlaylist);
    } else {
      this.document.body.classList.add(this.showPlaylist);
    }
  }

  ngOnInit(): void {
    this.retrieveStreamsCatalogueData();
  }

  private retrieveStreamsCatalogueData(): void {
    this.isLoading = true;
    this.streamsService.getStreamsWithFullDetails().subscribe((data) => {
      this.streamsForCatalogue = [];
      data
        .filter(
          (stream: any) => !stream.isDeleted && stream.streamTracks.length > 0
        )
        .map((stream: any) => {
          const streamForCatalogue = {
            id: stream.id,
            name: stream.name,
            imageFileName: stream.imageFileName,
            imageUrl: stream.imageUrl,
            tracks: stream.streamTracks.map((streamTrack: any) => {
              console.log('stream track');
              console.log(streamTrack);
              const mappedTrackDetails = {
                name: streamTrack.track.name,
                artist: streamTrack.track.album.performer.name,
                album: streamTrack.track.album.name,
                url: streamTrack.track.audioFileUrl,
                cover_art_url: streamTrack.track.album.imageUrl,
              } as ITrackDetailsDTO;
              return mappedTrackDetails;
            }),
          } as IStreamDetailsDTO;
          this.streamsForCatalogue.push(streamForCatalogue);
        });
      amplitude.init({
        songs: [...this.streamsForCatalogue[0].tracks],
      });
      console.log(amplitude);
      console.log('amplitude version');
      console.log(amplitude.getVersion());
      this.isLoading = false;
    });
  }
}

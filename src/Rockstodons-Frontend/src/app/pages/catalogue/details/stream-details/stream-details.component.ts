import { DOCUMENT } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
//@ts-ignore
import * as amplitude from 'amplitudejs';
import { take } from 'rxjs';
import { IStreamDetailsDTO } from 'src/app/core/interfaces/streams/stream-details-dto';
import { ITrackDetailsDTO } from 'src/app/core/interfaces/tracks/track-details-dto';
import { StreamsService } from 'src/app/core/services/streams.service';

@Component({
  selector: 'app-stream-details',
  templateUrl: './stream-details.component.html',
  styleUrls: ['./stream-details.component.scss']
})
export class StreamDetailsComponent {
  id!: string;
  streamDetails!: IStreamDetailsDTO;

  initialSong: any;
  currentTrack: any;
  volumeIcon = 'play-circle';
  showPlaylist = 'open-right-sidebar';
  playerClass = 'player-primary';

  fallback = '../../../assets/images/alternative-image.png';

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private streamsService: StreamsService,
    private activatedRoute: ActivatedRoute
  ) {
    this.id = this.activatedRoute.snapshot.paramMap.get('id')!;
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
  
  ngOnInit() {
    this.streamsService.getStreamById(this.id).pipe(
      take(1)
    ).subscribe((response: any) => {
      this.streamDetails = {
        id: response.id,
        name: response.name,
        imageFileName: response.imageFileName,
        imageUrl: response.imageFileName,
        tracks: response.streamTracks.map((streamTrack: any) => {
          const mappedTrackDetails = {
            name: streamTrack.track.name,
            artist: streamTrack.track.album.performer.name,
            album: streamTrack.track.album.name,
            url: streamTrack.track.audioFileUrl,
            cover_art_url: streamTrack.track.album.imageUrl,
          } as ITrackDetailsDTO;
          return mappedTrackDetails
        })
      } as IStreamDetailsDTO;
      amplitude.init({
        songs: [...this.streamDetails.tracks],
      });
    });
  }

}

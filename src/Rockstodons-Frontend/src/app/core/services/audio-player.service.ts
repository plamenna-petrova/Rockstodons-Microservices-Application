import { Injectable } from '@angular/core';
//@ts-ignore
import * as amplitude from 'amplitudejs';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AudioPlayerService {

  public playedTrackSubject = new Subject<any>();

  constructor() {

  }

  playTrack(trackToPlay: any) {
    this.playedTrackSubject.next(trackToPlay);
    amplitude.removeSong(0);
    amplitude.playNow(trackToPlay);
  }

  initCurrentStream(stream: any, trackIndex = 0) {
    const streamName = stream.name;
    amplitude.removeSong(0);
    if (!amplitude.getActivePlaylist()) {
      amplitude.addPlaylist(streamName, { name: streamName }, stream.tracks);
    }
    amplitude.playPlaylistSongAtIndex(trackIndex, streamName);
    const track = stream.tracks[trackIndex];
    amplitude.playNow(track);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ITrack } from '../interfaces/tracks/track';
import { ITrackCreateDTO } from '../interfaces/tracks/track-create-dto';
import { ITrackUpdateDTO } from '../interfaces/tracks/track-update-dto';

@Injectable({
  providedIn: 'root'
})
export class TracksService {
  private readonly tracksAPIUrl = `${environment.apiUrl}/tracks`;

  constructor(private httpClient: HttpClient) {

  }

  getAllTracks(): Observable<ITrack[]> {
    return this.httpClient.get<ITrack[]>(this.tracksAPIUrl);
  }

  getTracksWithFullDetails(): Observable<ITrack[]> {
    return this.httpClient.get<ITrack[]>(`${this.tracksAPIUrl}/all`);
  }

  getTrackByid(trackId: string): Observable<ITrack> {
    return this.httpClient.get<ITrack>(`${this.tracksAPIUrl}/${trackId}`);
  }

  getTrackDetails(trackDetailsId: string): Observable<ITrack> {
    return this.httpClient.get<ITrack>(`${this.tracksAPIUrl}/details/${trackDetailsId}`);
  }

  createNewTrack(trackToCreate: ITrackCreateDTO): Observable<ITrackCreateDTO> {
    return this.httpClient.post<ITrackCreateDTO>(`${this.tracksAPIUrl}/create`, trackToCreate);
  }

  updateTrack(trackToUpdate: ITrackUpdateDTO): Observable<ITrackUpdateDTO> {
    const updateTrackRequestBody = (({ id, ...tru }) => tru)(trackToUpdate);
    return this.httpClient.put<ITrackUpdateDTO>(
      `${this.tracksAPIUrl}/update/${trackToUpdate.id}`, updateTrackRequestBody
    );
  }

  deleteTrack(trackToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.tracksAPIUrl}/delete/${trackToDeleteId}`);
  }

  deleteTrackPermanently(trackToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.tracksAPIUrl}/confirm-deletion/${trackToDeletePermanentlyId}`
    );
  }

  restoreTrack(trackToRestoreId: string): Observable<void> {
    return this.httpClient.post<void>(
      `${this.tracksAPIUrl}/restore/${trackToRestoreId}`, null
    )
  }
}

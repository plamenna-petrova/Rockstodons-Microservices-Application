import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IGenre } from '../interfaces/genres/genre';
import { IGenreCreateDTO } from '../interfaces/genres/genre-create-dto';
import { IGenreUpdateDTO } from '../interfaces/genres/genre-update-dto';

@Injectable({
  providedIn: 'root'
})
export class GenresService {
  private readonly genresAPIUrl = `${environment.apiUrl}/genres`;

  constructor(private httpClient: HttpClient) {

  }

  getAllGenres(): Observable<IGenre[]> {
    return this.httpClient.get<IGenre[]>(this.genresAPIUrl);
  }

  getGenresWithFullDetails(): Observable<IGenre[]> {
    return this.httpClient.get<IGenre[]>(`${this.genresAPIUrl}/all`);
  }

  getGenreById(genreId: string): Observable<IGenre> {
    return this.httpClient.get<IGenre>(`${this.genresAPIUrl}/${genreId}`);
  }

  getGenreDetails(genreDetailsId: string): Observable<IGenre> {
    return this.httpClient.get<IGenre>(`${this.genresAPIUrl}/details/${genreDetailsId}`);
  }

  createNewGenre(genreToCreate: IGenreCreateDTO): Observable<IGenreCreateDTO> {
    return this.httpClient.post<IGenreCreateDTO>(`${this.genresAPIUrl}/create`, genreToCreate);
  }

  updateGenre(genreToUpdate: IGenreUpdateDTO): Observable<IGenreUpdateDTO> {
    const updateGenreRequestBody = (({ id, ...gtu }) => gtu)(genreToUpdate);
    return this.httpClient.put<IGenreUpdateDTO>(
      `${this.genresAPIUrl}/update/${genreToUpdate.id}`, updateGenreRequestBody
    );
  }

  deleteGenre(genreToDeleteId: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.genresAPIUrl}/delete/${genreToDeleteId}`);
  }

  deleteGenrePermanently(genreToDeletePermanentlyId: string): Observable<void> {
    return this.httpClient.delete<void>(
      `${this.genresAPIUrl}/confirm-deletion/${genreToDeletePermanentlyId}`
    );
  }

  restoreGenre(genreToRestoreId: string): Observable<void> {
    return this.httpClient.post<void>(
      `${this.genresAPIUrl}/restore/${genreToRestoreId}`, null
    );
  }
}

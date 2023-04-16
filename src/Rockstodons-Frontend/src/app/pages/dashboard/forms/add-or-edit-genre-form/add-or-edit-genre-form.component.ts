import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-add-or-edit-genre-form',
  templateUrl: './add-or-edit-genre-form.component.html',
  styleUrls: ['./add-or-edit-genre-form.component.scss']
})
export class AddOrEditGenreFormComponent {
  @Input() genreActionFormGroup!: FormGroup;
}

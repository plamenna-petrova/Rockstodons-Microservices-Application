import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-add-or-edit-album-type-form',
  templateUrl: './add-or-edit-album-type-form.component.html',
  styleUrls: ['./add-or-edit-album-type-form.component.scss']
})
export class AddOrEditAlbumTypeFormComponent {
  @Input() albumTypeActionForm!: FormGroup;
}

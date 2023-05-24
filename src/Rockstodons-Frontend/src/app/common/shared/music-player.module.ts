import { NgModule } from '@angular/core';
import { MusicPlayerComponent } from 'src/app/pages/catalogue/music-player/music-player.component';
import { CommonModule } from '@angular/common';
import { TimeConversionPipe } from 'src/app/core/pipes/time-conversion.pipe';

@NgModule({
  declarations: [MusicPlayerComponent, TimeConversionPipe],
  imports: [
    CommonModule
  ],
  exports: [MusicPlayerComponent]
})
export class MusicPlayerModule { }

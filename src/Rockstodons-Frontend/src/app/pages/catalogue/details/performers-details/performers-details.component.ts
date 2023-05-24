import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NzDescriptionsSize } from 'ng-zorro-antd/descriptions';
import { take } from 'rxjs';
import { IPerformer } from 'src/app/core/interfaces/performers/performer';
import { PerformersService } from 'src/app/core/services/performers.service';

@Component({
  selector: 'app-performers-details',
  templateUrl: './performers-details.component.html',
  styleUrls: ['./performers-details.component.scss']
})
export class PerformersDetailsComponent {
  id!: string;
  performerDetails!: IPerformer;
  performerDescriptionsDetailsTitle!: string;
  size: NzDescriptionsSize = 'default';

  fallback = '../../../assets/images/alternative-image.png';

  constructor(
    private performersService: PerformersService,
    private activatedRoute: ActivatedRoute
  ) {
    this.id = this.activatedRoute.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.performersService.getPerformerById(this.id).pipe(
      take(1)
    ).subscribe((response) => {
      this.performerDetails = response;
      this.performerDescriptionsDetailsTitle = `${this.performerDetails.name}\`s Details`;
    });
  }
}

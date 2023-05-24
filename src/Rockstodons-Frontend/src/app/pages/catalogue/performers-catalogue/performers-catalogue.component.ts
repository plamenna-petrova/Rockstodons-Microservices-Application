import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IPerformer } from 'src/app/core/interfaces/performers/performer';
import { PerformersService } from 'src/app/core/services/performers.service';

@Component({
  selector: 'app-performers-catalogue',
  templateUrl: './performers-catalogue.component.html',
  styleUrls: ['./performers-catalogue.component.scss']
})
export class PerformersCatalogueComponent {
  performersForCatalogue!: IPerformer[];

  isLoading = false;

  gridSelectionStyle = {
    width: '95%',
    textAlign: 'center'
  }

  fallback = '../../../assets/images/alternative-image.png';

  constructor(
    private performersService: PerformersService,
    private router: Router
  ) {

  }

  getPerformerDetails(performerId: string): void {
    const performerDetailsRouteToNavigate = `/performer-details/${performerId}`
    this.router.navigate([`${performerDetailsRouteToNavigate}`]);
  }

  ngOnInit(): void {
    this.retrievePerformersCatalogueData();
  }

  private retrievePerformersCatalogueData(): void {
    this.isLoading = true;
    this.performersService.getAllPerformers().subscribe((data) => {
      this.performersForCatalogue = [...data];
      this.isLoading = false;
    });
  }
}

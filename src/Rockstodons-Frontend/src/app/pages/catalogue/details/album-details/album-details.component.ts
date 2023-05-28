import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { format, parse, parseISO } from 'date-fns';
import { NzDescriptionsSize } from 'ng-zorro-antd/descriptions';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Observable, take } from 'rxjs';
import { IAlbum } from 'src/app/core/interfaces/albums/album';
import { IComment } from 'src/app/core/interfaces/comments/comment';
import { ICommentCreateDTO } from 'src/app/core/interfaces/comments/comment-create-dto';
import { ISubcommentCreateDTO } from 'src/app/core/interfaces/subcomments/subcomment-create-dto';
import { AlbumsService } from 'src/app/core/services/albums.service';
import { CommentsService } from 'src/app/core/services/comments.service';
import { OAuth2Service } from 'src/app/core/services/oauth2.service';
import { SubcommentsService } from 'src/app/core/services/subcomments.service';
import { operationSuccessMessage } from 'src/app/core/utils/global-constants';

@Component({
  selector: 'app-album-details',
  templateUrl: './album-details.component.html',
  styleUrls: ['./album-details.component.scss'],
})
export class AlbumDetailsComponent {
  id!: string;
  albumDetails!: IAlbum;
  albumDescriptionsDetailsTitle!: string;
  size: NzDescriptionsSize = 'default';

  isAuthenticated: Observable<boolean>;

  fallback = '../../../assets/images/alternative-image.png';

  albumComments!: IComment[];
  commentCreationForm!: FormGroup;
  subcommentCreationForm!: FormGroup;
  isCommentSubmitted = false;
  isSubcommentCreationModalVisible = false;

  constructor(
    private albumsService: AlbumsService,
    private oauth2Service: OAuth2Service,
    private commentsService: CommentsService,
    private subcommentsService: SubcommentsService,
    private nzNotificationService: NzNotificationService,
    private activatedRoute: ActivatedRoute
  ) {
    this.isAuthenticated = oauth2Service.isAuthenticated$;
    this.id = this.activatedRoute.snapshot.paramMap.get('id')!;
    this.buildCommentsActionForms();
  }

  buildCommentsActionForms(): void {
    this.commentCreationForm = new FormGroup<ICommentActionForm>({
      content: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(200)
        ]),
        nonNullable: true
      })
    });
    this.subcommentCreationForm = new FormGroup<ICommentActionForm>({
      content: new FormControl('', {
        validators: Validators.compose([
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(200)
        ]),
        nonNullable: true
      })
    });
  }

  handleAlbumCommentSubmit(): void {
    this.isCommentSubmitted = true;
    const commentContent: string = this.commentCreationForm.value.content;

    setTimeout(() => {
      this.isCommentSubmitted = false;

      const commentToCreate: ICommentCreateDTO = {
        content: commentContent,
        userId: this.oauth2Service.identityClaims['sub'],
        author: this.oauth2Service.identityClaims['name'],
        albumId: this.albumDetails.id,
      };

      this.commentsService
        .createNewComment(commentToCreate)
        .pipe(take(1))
        .subscribe({
          next: () => {
            this.nzNotificationService.success(
              operationSuccessMessage,
              `The comment is created!`,
              {
                nzPauseOnHover: true,
              }
            );

            this.retrieveAlbumDetails();
          },
          error: (error) => {
            console.log(error);
          },
        });
    }, 800);
  }

  showSubcommentCreationModal(): void {
    this.isSubcommentCreationModalVisible = true;
    console.log('here');
  }

  handleOkSubcommentCreationModal(comment: IComment): void {
    this.handleSubcommentSubmit(comment);
  }

  handleCancelSubcommentCreationModal(): void {
    this.isSubcommentCreationModalVisible = false;
  }

  handleSubcommentSubmit(comment: IComment): void {
    const subcommentContent: string = this.subcommentCreationForm.value.content;

    const subcommentToCreate: ISubcommentCreateDTO = {
      content: subcommentContent,
      userId: this.oauth2Service.identityClaims['sub'],
      author: this.oauth2Service.identityClaims['name'],
      commentId: comment.id
    };

    this.subcommentsService
      .createNewSubcomment(subcommentToCreate)
      .pipe(take(1))
      .subscribe({
        next: () => {
          this.nzNotificationService.success(
            operationSuccessMessage,
            `Reply sent`,
            {
              nzPauseOnHover: true
            }
          );

          this.retrieveAlbumDetails();
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {
          this.isSubcommentCreationModalVisible = false;
        }
      })
  }

  private retrieveAlbumDetails(): void {
    this.albumsService
    .getAlbumById(this.id)
    .pipe(take(1))
    .subscribe((response) => {
      this.albumDetails = response;
      console.log('album details');
      this.albumComments = [...this.albumDetails.comments];
      this.albumComments = this.albumComments.map(comment => {
        comment.createdOn = format(
          parseISO(comment.createdOn.toString()), "dd-MM-yyyy HH:mm:ss"
        )
        return comment;
      });
      this.albumDescriptionsDetailsTitle = `${this.albumDetails.name}\`s Details`;
    });
  }

  ngOnInit(): void {
    this.retrieveAlbumDetails();
  }
}

export interface ICommentActionForm {
  content: FormControl<string>;
}

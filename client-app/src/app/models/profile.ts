export interface IProfile {
    displayName: string,
    username: string,
    bio: string,
    image: string,
    photos: IPhoto[],
    following: boolean,
    followersCount: number,
    followingCount: number
};

export interface IPhoto {
    id: string,
    url: string,
    isMain: boolean
}

export interface IProfileFormValues {
    displayName: string;
    bio: string;
}

export class EditProfileFormValues implements IProfileFormValues {
    displayName: string = '';
    bio: string = '';

    constructor(init?: IProfile) {
        if (init) {
            this.displayName = init.displayName;
            this.bio = init.bio;
        }
    }
}

export interface IUserActivity {
    id: string;
    title: string;
    category: string;
    date: Date;
}
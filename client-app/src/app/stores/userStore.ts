import { observable, computed, action, runInAction } from "mobx";
import { IUser, IUserFormValues } from "../models/user";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from "../..";

export default class UserStore {
    rootStore: RootStore;
    constructor(rootStore: RootStore) {
        this.rootStore = rootStore;
    }
    refeshTokenTimeout: any;

    @observable user: IUser | null = null;
    @observable loading = false;

    @computed get isLoggedIn() { return !!this.user }

    @action login = async (values: IUserFormValues) => {
        try {
            const user = await agent.User.login(values);
            runInAction(() => {
                this.user = user;

            });
            this.rootStore.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);
            this.rootStore.modalStore.closeModal();
            history.push('/activities');
        }
        catch (error) {
            throw error;
        }
    }

    @action register = async (values: IUserFormValues) => {
        try {
            const user = await agent.User.register(values);
            this.rootStore.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);
            this.rootStore.modalStore.closeModal();
            history.push('/activities');
        }
        catch (error) {
            throw error;
        }
    }

    @action getUser = async () => {
        try {
            const user = await agent.User.currentUser();
            runInAction(() => {
                this.user = user;
            });
            this.rootStore.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);
        }
        catch (error) {
            console.log(error);
        }
    }

    @action logOut = () => {
        this.rootStore.commonStore.setToken(null);
        this.user = null;
        if (this.refeshTokenTimeout) {
            console.log('clearing refreshTokentimeout');
            this.stopRefreshTokenTimer();
        }
        
        history.push('/');
    }

    @action fbLogin = async (response: any) => {
        this.loading = true;
        try {
            const user = await agent.User.fbLogin(response.accessToken);
            runInAction(() => {
                this.user = user;
                this.rootStore.commonStore.setToken(user.token);
                this.startRefreshTokenTimer(user);
                this.rootStore.modalStore.closeModal();
                this.loading = false;
            })
            history.push('/activities');
        }
        catch (error) {
            this.loading = false;
            throw error;
        }
    }

    @action refreshToken = async () => {
        this.stopRefreshTokenTimer();
        try {
            const user = await agent.User.refreshToken();
            runInAction(() => {
                this.user = user;
            });
            this.rootStore.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);
        }
        catch (error) {
            console.log(error);
        }
    }

    private startRefreshTokenTimer(user: IUser) {
        const jwtToken = JSON.parse(atob(user.token.split('.')[1]));
        const expires = new Date(jwtToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - (60 * 1000);
        this.refeshTokenTimeout = setTimeout(this.refreshToken, timeout);
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refeshTokenTimeout);
    }
}